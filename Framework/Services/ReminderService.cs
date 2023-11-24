using AutoMapper;
using Core.Validation;
using Framework.DomainModels.Base;
using Framework.DomainModels.Common;
using Framework.Extensions;
using Framework.Repositories;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using ToDo.Data.ToDoData.Entities;

namespace Framework.Services
{
    public class ReminderService
    {
        private readonly ItemRepository _itemRepository;
        private readonly UserRepository _userRepository;
        private readonly IdentityRepository _identityRepository;
        private readonly EmailService _emailService;
        private readonly IMapper _mapper;

        public ReminderService(ItemRepository repository, UserRepository userRepository, IdentityRepository identityRepository, EmailService emailService, IMapper mapper)
        {
            _itemRepository = repository;
            _userRepository = userRepository;
            _identityRepository = identityRepository;
            _emailService = emailService;
            _mapper = mapper;
        }


        public async Task UpdateReminders(Guid itemId)
        {
            var item = await _itemRepository.GetItemCompleteAsync(itemId);
            if (item == null)
                return;

            var list = await _itemRepository.GetAsync<ToDoList>(item.ListId);
            if (list == null)
                return;

            var userIDs = list.UserId is not null ? new [] { list.UserId.Value }.ToList() : (await _userRepository.GetAllUsersForGroupAsync(list.GroupId!.Value)).Select(u => u.UserId).ToList();
            var userEmails = new List<string>();
            if (list.UserId is not null)
            {
                var identityUser = await _identityRepository.GetAsync<IdentityUser>(list.UserId.Value.ToString());
                identityUser?.Email.NotNull();

                userEmails.Add(identityUser!.Email!);
            }
            else
            {
                var groupUserIds = (await _userRepository.GetAllUsersForGroupAsync(list.GroupId!.Value)).Select(u => u.UserId.ToString()).ToList();
                var userIds = (await _identityRepository.GetUsersByIdAsync(groupUserIds)).Select(i => i.Email ?? string.Empty).Where(e => !string.IsNullOrWhiteSpace(e));
                userEmails.AddRange(userIds);
            }

            var schedules = item.Schedules.ToList();
            var reminders = item.Reminders.ToList();

            var scheduleDomainModels = _mapper.Map<List<ScheduleDomainModel>>(schedules);

            var existingHangfireJobs = await _itemRepository.GetAllAsync<HangfireJob>(j => j.ToDoItemId == itemId);
            foreach (var job in existingHangfireJobs)
            {
                BackgroundJob.Delete(job.JobId);
                RecurringJob.RemoveIfExists(job.JobId);
            }

            if (existingHangfireJobs.Any())
                await _itemRepository.RemoveAndSaveAsync(existingHangfireJobs.ToArray());

            if (item.InactiveSince.HasValue)
                return;

            var newJobIds = new List<string>();
            foreach (var schedule in scheduleDomainModels)
            {
                var nextScheduleOccurrence = schedule.ScheduleDefinition.NextOccurrenceAfter(DateTime.Now, schedule.Start, schedule.End);
                if (nextScheduleOccurrence == null)
                    continue;

                foreach (var reminder in reminders)
                {
                    var reminderDefinition = _mapper.Map<ReminderDefinition>(reminder.Definition);
                    var nextReminderTime = reminderDefinition.ApplyReminderToOccurrence(nextScheduleOccurrence.Value);
                    if (schedule.Type == DomainModels.Common.Enums.ScheduleType.Fixed)
                    {
                        if (nextScheduleOccurrence == null)
                            continue;

                        if (nextReminderTime < DateTime.Now)
                            continue;

                        var newJobId = BackgroundJob.Schedule(() => _emailService.SendReminderAsync(itemId, userEmails.ToArray()), nextReminderTime);
                        newJobIds.Add(newJobId);
                    }
                    else
                    {
                        if (schedule.ScheduleDefinition.WeekDays != null)
                            schedule.ScheduleDefinition.WeekDays.Time = TimeOnly.FromDateTime(reminderDefinition.ApplyReminderToOccurrence(new DateTime(schedule.ScheduleDefinition.WeekDays.Time.Ticks)));

                        schedule.Start = nextReminderTime;

                        var cronDefinition = _mapper.Map<CronDomainModel>(schedule);

                        var newJob = reminder.Id.ToString().ToLower();
                        RecurringJob.AddOrUpdate(newJob, () => _emailService.SendReminderAsync(itemId, userEmails.ToArray()), cronDefinition, new RecurringJobOptions() { TimeZone = TimeZoneInfo.Local });

                        newJobIds.Add(newJob);
                    }
                }
            }

            if (newJobIds.Any())
                await _itemRepository.AddAndSaveAsync(newJobIds.Select(j => new HangfireJob() { JobId = j, ToDoItemId = itemId }).ToArray());
        }
    }
}
