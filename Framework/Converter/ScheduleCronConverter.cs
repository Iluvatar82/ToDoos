using AutoMapper;
using Core.Validation;
using Framework.DomainModels.Base;
using Framework.DomainModels.Common;

namespace Framework.Converter
{
    public class ScheduleCronConverter : IValueConverter<ScheduleDomainModel, CronDomainModel>, ITypeConverter<ScheduleDomainModel, CronDomainModel>
    {
        public CronDomainModel Convert(ScheduleDomainModel schedule, ResolutionContext context)
        {
            schedule.NotNull();

            if (schedule.Type == DomainModels.Common.Enums.ScheduleType.WeekDays)
                return $"{schedule.ScheduleDefinition.WeekDays!.Time.Minute} {schedule.ScheduleDefinition.WeekDays!.Time.Hour} * * {schedule.ScheduleDefinition.WeekDays!.DaysDefinition}";

            schedule.Type.Satisfies(t => t == DomainModels.Common.Enums.ScheduleType.Interval);
            if (schedule.ScheduleDefinition.Interval!.Unit == DomainModels.Common.Enums.ScheduleTimeUnit.Minute)
                return $"*/{schedule.ScheduleDefinition.Interval.Interval} * * * *";
 
            if (schedule.ScheduleDefinition.Interval!.Unit == DomainModels.Common.Enums.ScheduleTimeUnit.Hour)
                return $"{schedule.Start!.Value.Minute} */{schedule.ScheduleDefinition.Interval.Interval} * * *";
            
            if (schedule.ScheduleDefinition.Interval!.Unit == DomainModels.Common.Enums.ScheduleTimeUnit.Day)
                return $"{schedule.Start!.Value.Minute} {schedule.Start.Value.Hour} */{schedule.ScheduleDefinition.Interval.Interval} * *";
            
            if (schedule.ScheduleDefinition.Interval!.Unit == DomainModels.Common.Enums.ScheduleTimeUnit.Month)
                return $"{schedule.Start!.Value.Minute} {schedule.Start.Value.Hour} {schedule.Start.Value.Day} */{schedule.ScheduleDefinition.Interval.Interval} *";
            
            if (schedule.ScheduleDefinition.Interval!.Unit == DomainModels.Common.Enums.ScheduleTimeUnit.Year)
                return $"{schedule.Start!.Value.Minute} {schedule.Start.Value.Hour} {schedule.Start!.Value.Day} {schedule.Start!.Value.Month} *";

            return "* * * * *";
        }

        public CronDomainModel Convert(ScheduleDomainModel source, CronDomainModel destination, ResolutionContext context) => Convert(source, context);
    }
}
