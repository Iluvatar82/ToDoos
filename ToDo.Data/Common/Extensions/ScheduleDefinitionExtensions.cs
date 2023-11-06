using System.Diagnostics.Metrics;
using static System.Net.Mime.MediaTypeNames;

namespace ToDo.Data.Common.Extensions
{
    public static class ScheduleDefinitionExtensions
    {
        public static DateTime? NextOccurrenceAfter(this ScheduleDefinition schedule, DateTime after, DateTime? from = null, DateTime? to = null)
        {
            if (schedule.Fixed is not null)
                return schedule.Fixed >= after ? schedule.Fixed : null;

            if (schedule.WeekDays is not null)
            {
                var timeofDay = schedule.WeekDays.Time.ToTimeSpan();
                var oneWeek = Enumerable.Range(0, 7).Select(d => after.Date.AddDays(d).AddTicks(timeofDay.Ticks));
                var activeWeekDays = schedule.WeekDays.Days.Select((a, index) => (Active: a, WeekDay: (DayOfWeek)index)).Where(info => info.Active).Select(info => info.WeekDay).ToList();

                DateTime? next = oneWeek.FirstOrDefault(date => activeWeekDays.Any(active => active == date.DayOfWeek) && date.TimeOfDay >= timeofDay);
                if (from.HasValue)
                    next = next >= from ? next : null;

                if (to.HasValue)
                    next = next < to ? next : null;

                return next;
            }

            if (schedule.Interval is not null)
            {
                if (from == null)
                    return null;

                var firstOccurrenceAfter = from.Value.AddDays(Math.Ceiling((after - from.Value).TotalDays / (double)schedule.Interval.Interval) * (double)schedule.Interval.Interval);
                if (to < firstOccurrenceAfter) 
                    return null;

                return firstOccurrenceAfter;
            }

            return null;
        }

        public static DateTime? LastOccurrenceBefore(this ScheduleDefinition schedule, DateTime before, DateTime? from = null, DateTime? to = null)
        {
            if (schedule.Fixed is not null)
                return schedule.Fixed <= before ? schedule.Fixed : null;

            if (schedule.WeekDays is not null)
            {
                var timeofDay = schedule.WeekDays.Time.ToTimeSpan();
                var oneWeek = Enumerable.Range(0, 7).Select(d => before.Date.AddDays(-d).AddTicks(timeofDay.Ticks));
                var activeWeekDays = schedule.WeekDays.Days.Select((a, index) => (Active: a, WeekDay: (DayOfWeek)index)).Where(info => info.Active).Select(info => info.WeekDay).ToList();

                DateTime? previous = oneWeek.FirstOrDefault(date => activeWeekDays.Any(active => active == date.DayOfWeek) && date.TimeOfDay <= timeofDay);
                if (to.HasValue)
                    previous = previous < to ? previous : null;

                if (from.HasValue)
                    previous = previous >= from ? previous : null;

                return previous;
            }

            if (schedule.Interval is not null)
            {
                if (from == null)
                    return null;

                var lastOccurrenceBefore = from.Value.AddDays(-Math.Floor((before - from.Value).TotalDays / (double)schedule.Interval.Interval) * (double)schedule.Interval.Interval);
                if (to < lastOccurrenceBefore)
                    return null;

                return lastOccurrenceBefore;
            }

            return null;
        }
    }
}
