using AutoMapper;
using Framework.DomainModels;
using Framework.DomainModels.Base;
using Framework.DomainModels.Common;
using Framework.DomainModels.Common.Enums;
using ToDo.Data.ToDoData.Entities;

namespace Framework.Converter.Automapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile() {

            CreateMap<HangfireJob, HangfireJobDomainModel>();
            CreateMap<HangfireJobDomainModel, HangfireJob>();

            CreateMap<Setting, SettingDomainModel>();
            CreateMap<SettingDomainModel, Setting>();

            CreateMap<Invitation, InvitationDomainModel>();
            CreateMap<InvitationDomainModel, Invitation>();

            CreateMap<Notification, NotificationDomainModel>();
            CreateMap<NotificationDomainModel, Notification>();

            CreateMap<string, ScheduleTimeUnit>().ConvertUsing<TimeUnitConverter>();
            CreateMap<ScheduleTimeUnit, string>().ConvertUsing<TimeUnitConverter>();

            CreateMap<string, ReminderDefinition>().ConvertUsing<ScheduleReminderDefinitionConverter>();
            CreateMap<ReminderDefinition, string>().ConvertUsing<ScheduleReminderDefinitionConverter>();

            CreateMap<string, ScheduleDefinition>().ConvertUsing<ScheduleDefinitionConverter>();
            CreateMap<ScheduleDefinition, string>().ConvertUsing<ScheduleDefinitionConverter>();

            CreateMap<ScheduleReminder, ScheduleReminderDomainModel>().ForMember(dest => dest.ReminderDefinition, opt => opt.MapFrom(src => src.Definition));
            CreateMap<ScheduleReminderDomainModel, ScheduleReminder>().ForMember(dest => dest.Definition, opt => opt.MapFrom(src => src.ReminderDefinition));

            CreateMap<Schedule, ScheduleDomainModel>();
            CreateMap<ScheduleDomainModel, Schedule>();
            CreateMap<ScheduleDomainModel, CronDomainModel>().ConvertUsing<ScheduleCronConverter>();

            CreateMap<UserGroup, UserGroupDomainModel>();
            CreateMap<UserGroupDomainModel, UserGroup>();

            CreateMap<Category, CategoryDomainModel>();
            CreateMap<CategoryDomainModel, Category>();

            CreateMap<ToDoList, ToDoListDomainModel>();
            CreateMap<ToDoListDomainModel, ToDoList>();

            CreateMap<ToDoItem, ToDoItemDomainModel>();
            CreateMap<ToDoItemDomainModel, ToDoItem>()
                .ForMember(dest => dest.Parent, opt => opt.Ignore())
                .ForMember(dest => dest.Children, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore());
        }
    }
}
