using ToDo.Data.Common.Enums;

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

                var firstOccurrenceAfter = CalculateIntervalOccurrence(schedule, start.Value, after);
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

                var lastOccurrenceBefore = CalculateIntervalOccurrence(schedule, start.Value, before);
                if (end < lastOccurrenceBefore)
                    return null;

                return lastOccurrenceBefore;
            }

            return null;
        }

        private static DateTime CalculateIntervalOccurrence(ScheduleDefinition schedule, DateTime start, DateTime comparison, bool after = true)
        {
            Func<DateTime, double, DateTime> timeIntervalFunc = schedule.Interval!.Unit switch
            {
                ScheduleTimeUnit.Hour => (c, i) => c.AddHours(i),
                ScheduleTimeUnit.Day => (c, i) => c.AddDays(i),
                ScheduleTimeUnit.Week => (c, i) => c.AddDays(i * 7),
                ScheduleTimeUnit.Month => (c, i) => c.AddMonths((int)i),
                ScheduleTimeUnit.Year => (c, i) => c.AddYears((int)i),
                _ => (c, i) => c
            };

            if (after && comparison < start)
                return start;
            else if (!after && comparison > start)
                return start;

            var intervalFactor = (double)schedule.Interval.Interval * (after ? 1 : -1);
            var currentValue = start;
            while(true)
            {
                currentValue = timeIntervalFunc(currentValue, intervalFactor);
                if (after && currentValue > comparison || !after && currentValue < comparison)
                    break;
            }

            return currentValue;
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
                        currentCheckDate = currentCheckDate.AddDays((double)schedule.Interval.Interval);
                    }
                }
            }

            return result;
        }
    }
}
