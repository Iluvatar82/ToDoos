namespace ToDo.Data.Common.Extensions
{
    public static class ScheduleDefinitionExtensions
    {
        public static DateTime? NextOccurrenceAfter(this ScheduleDefinition schedule, DateTime after)
        {
            if (schedule.Fixed is not null)
                return schedule.Fixed > after ? schedule.Fixed : null;

            if (schedule.WeekDays is not null)
            {
                var timeofDay = schedule.WeekDays.Time.ToTimeSpan();
                var oneWeek = Enumerable.Range(0, 7).Select(d => after.Date.AddDays(d).AddTicks(timeofDay.Ticks));
                var activeWeekDays = schedule.WeekDays.Days.Select((a, index) => (Active: a, WeekDay: (DayOfWeek)index)).Where(info => info.Active).Select(info => info.WeekDay).ToList();

                var next = oneWeek.FirstOrDefault(date => activeWeekDays.Any(active => active == date.DayOfWeek) && date.TimeOfDay >= timeofDay);
                return next;
            }

            if (schedule.Interval is not null)
            {
                //TODO
                return null;
            }

            return null;
        }
    }
}
