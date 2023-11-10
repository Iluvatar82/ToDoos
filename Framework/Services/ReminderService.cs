using Framework.Repositories;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using ToDo.Data.Common.Converter;
using ToDo.Data.Common.Extensions;
using ToDo.Data.ToDoData.Entities;

namespace Framework.Services
{
    public class ReminderService
    {
        private readonly ItemRepository _itemRepository;
        private readonly UserRepository _userRepository;
        private readonly IdentityRepository _identityRepository;
        private readonly EmailService _emailService;

        public ReminderService(ItemRepository repository, UserRepository userRepository, IdentityRepository identityRepository, EmailService emailService)
        {
            _itemRepository = repository;
            _userRepository = userRepository;
            _identityRepository = identityRepository;
            _emailService = emailService;
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
                userEmails.Add((await _identityRepository.GetAsync<IdentityUser>(list.UserId.Value.ToString())).Email);
            else
            {
                var groupUserIds = (await _userRepository.GetAllUsersForGroupAsync(list.GroupId!.Value)).Select(u => u.UserId.ToString()).ToList();
                var userIds = (await _identityRepository.GetUsersByIdAsync(groupUserIds)).Select(i => i.Email ?? string.Empty).Where(e => !string.IsNullOrWhiteSpace(e));
                userEmails.AddRange(userIds);
            }

            var schedules = item.Schedules.ToList();
            var reminders = item.Reminders.ToList();

            var nextScheduleOccurrences = schedules.Select(s => ScheduleDefinitionConverter.Convert(s.ScheduleDefinition).NextOccurrenceAfter(DateTime.Now, s.Start, s.End)).Where(d => d != null).Select(d => d.Value).OrderBy(d => d).ToList();
            var existingHangfireJobs = await _itemRepository.GetAllAsync<HangfireJob>(j => j.ToDoItemId == itemId);
            
            foreach (var job in existingHangfireJobs)
                RecurringJob.RemoveIfExists(job.JobId);

            if (existingHangfireJobs.Any())
                await _itemRepository.RemoveAndSaveAsync(existingHangfireJobs.ToArray());

            var newJobIds = new List<string>();
            foreach (var nextScheduleOccurrence in nextScheduleOccurrences)
            {
                foreach (var reminder in reminders)
                {
                    var reminderDefinition = ReminderConverter.Convert(reminder.Definition);
                    var nextReminderTime = reminderDefinition.ApplyReminderToOccurrence(nextScheduleOccurrence);

                    if (nextReminderTime < DateTime.Now)
                        continue;

                    //EmailBuilder-Service implementieren und hier vorlagern
                    //Save JobIDs in new Table
                    var newJob = BackgroundJob.Schedule(() => _emailService.SendAsync("Ihre Erinnerung",
                        $"<h4>Erinnerung!</h4><div style='font-weight:bold'>\"{item.Bezeichnung}\"!</div><div>Die Deadline ist am <span style='font-weight:bold'>{nextScheduleOccurrence.ToShortDateString()}</span> um <span style='font-weight:bold'>{nextScheduleOccurrence.ToShortTimeString()}!</span></div>",
                        Base.MessageType.Info, null, userEmails.ToArray()), nextReminderTime);

                    newJobIds.Add(newJob);
                }
            }

            if (newJobIds.Any())
                await _itemRepository.AddAndSaveAsync(newJobIds.Select(j => new HangfireJob() { JobId = j, ToDoItemId = itemId }).ToArray());
        }
    }
}
