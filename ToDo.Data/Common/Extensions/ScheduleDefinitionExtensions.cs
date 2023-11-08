using System.ComponentModel.Design;
using ToDo.Data.ToDoData.Entities;

namespace ToDo.Data.Common.Extensions
{
    public static class ScheduleDefinitionExtensions
    {
        public static DateTime? NextOccurrenceAfter(this ScheduleDefinition schedule, DateTime after, DateTime? start = null, DateTime? end = null)
        {
            if (schedule.Fixed is not null)
                return schedule.Fixed >= after ? schedule.Fixed : null;
            else if (schedule.WeekDays is not null)
            {
                var timeofDay = schedule.WeekDays.Time.ToTimeSpan();
                var oneWeek = Enumerable.Range(0, 7).Select(d => after.Date.AddDays(d).AddTicks(timeofDay.Ticks));
                var activeWeekDays = schedule.WeekDays.Days.Select((a, index) => (Active: a, WeekDay: (DayOfWeek)index)).Where(info => info.Active).Select(info => info.WeekDay).ToList();

                DateTime? next = oneWeek.FirstOrDefault(date => activeWeekDays.Any(active => active == date.DayOfWeek) && date.TimeOfDay >= timeofDay);
                if (start.HasValue)
                    next = next >= start ? next : null;

                if (end.HasValue)
                    next = next < end ? next : null;

                return next;
            }
            else if (schedule.Interval is not null)
            {
                if (start == null)
                    return null;

                var firstOccurrenceAfter = start.Value.AddDays(Math.Ceiling((after - start.Value).TotalDays / (double)schedule.Interval.Interval) * (double)schedule.Interval.Interval);
                if (end < firstOccurrenceAfter) 
                    return null;

                return firstOccurrenceAfter;
            }

            return null;
        }

        public static DateTime? LastOccurrenceBefore(this ScheduleDefinition schedule, DateTime before, DateTime? start = null, DateTime? end = null)
        {
            if (schedule.Fixed is not null)
                return schedule.Fixed <= before ? schedule.Fixed : null;
            else if (schedule.WeekDays is not null)
            {
                var timeofDay = schedule.WeekDays.Time.ToTimeSpan();
                var oneWeek = Enumerable.Range(0, 7).Select(d => before.Date.AddDays(-d).AddTicks(timeofDay.Ticks));
                var activeWeekDays = schedule.WeekDays.Days.Select((a, index) => (Active: a, WeekDay: (DayOfWeek)index)).Where(info => info.Active).Select(info => info.WeekDay).ToList();

                DateTime? previous = oneWeek.FirstOrDefault(date => activeWeekDays.Any(active => active == date.DayOfWeek) && date.TimeOfDay <= timeofDay);
                if (end.HasValue)
                    previous = previous < end ? previous : null;

                if (start.HasValue)
                    previous = previous >= start ? previous : null;

                return previous;
            }
            else if (schedule.Interval is not null)
            {
                if (start == null)
                    return null;

                var lastOccurrenceBefore = start.Value.AddDays(-Math.Floor((before - start.Value).TotalDays / (double)schedule.Interval.Interval) * (double)schedule.Interval.Interval);
                if (end < lastOccurrenceBefore)
                    return null;

                return lastOccurrenceBefore;
            }

            return null;
        }

        public static List<DateTime> GetOccurrences(this ScheduleDefinition schedule, DateTime from, DateTime to, DateTime? start = null, DateTime? end = null)
        {
            var result = new List<DateTime>();
            if (start > to || end < from)
                return result;

            if (schedule.Fixed is not null)
                result.Add(schedule.Fixed.Date);
            else
            {
                var checkDate = from;
                if (start.HasValue && start > from)
                    checkDate = start.Value;

                var lastCheckDate = to.AddDays(1).AddSeconds(-1);
                if (end.HasValue && end < to)
                    lastCheckDate = end.Value.AddDays(1).AddSeconds(-1);

                if (schedule.WeekDays is not null)
                {
                    var activeWeekdays = schedule.WeekDays.Days.Select((a, index) => (Active: a, Index: index)).Where(di => di.Active).Select(di => (DayOfWeek)di.Index).ToList();
                    for (var currentCheckDate = checkDate; currentCheckDate <= lastCheckDate; currentCheckDate = currentCheckDate.AddDays(1))
                    {
                        if (activeWeekdays.Contains(currentCheckDate.DayOfWeek))
                        {
                            var checkDateToAdd = currentCheckDate.AddTicks(schedule.WeekDays.Time.Ticks);
                            result.Add(checkDateToAdd);
                        }
                    }
                }
                else if (schedule.Interval is not null)
                {
                    var currentCheckDate = checkDate;
                    while(currentCheckDate <= lastCheckDate)
                    {
                        result.Add(currentCheckDate);
                        currentCheckDate = currentCheckDate.AddDays((double)schedule.Interval);
                    }
                }
            }

            return result;
        }
    }
}
