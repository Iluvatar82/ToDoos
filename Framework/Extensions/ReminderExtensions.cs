using ToDo.Data.Common;
using ToDo.Data.Common.Enums;

namespace Framework.Extensions
{
    public static class ReminderExtensions
    {
        public static DateTime ApplyReminderToOccurrence(this ReminderDefinition definition, DateTime occurrence)
        {
            Func<DateTime, double, DateTime> timeIntervalFunc = definition.Unit switch
            {
                ScheduleTimeUnit.Minute => (c, i) => c.AddMinutes(i),
                ScheduleTimeUnit.Hour => (c, i) => c.AddHours(i),
                ScheduleTimeUnit.Day => (c, i) => c.AddDays(i),
                ScheduleTimeUnit.Week => (c, i) => c.AddDays(i * 7),
                ScheduleTimeUnit.Month => (c, i) => c.AddMonths((int)i),
                ScheduleTimeUnit.Year => (c, i) => c.AddYears((int)i),
                _ => (c, i) => c
            };

            return timeIntervalFunc(occurrence, (double)-definition.Value);
        }
    }
}
