using ToDo.Data.Common.Converter;
using ToDo.Data.ToDoData.Entities;

namespace ToDo.Data.Common.Extensions
{
    public static class SchedulesExtensions
    {
        public static DateTime? NextOccurrenceAfter(this IEnumerable<Schedule> schedules, DateTime? after = null)
        {
            if (!schedules.Any())
                return null;

            var afterValue = after ?? DateTime.Now;
            var scheduleDefinitions = schedules.Select(s => ScheduleDefinitionConverter.Convert(s.ScheduleDefinition));

            var result = scheduleDefinitions.Select(s => s.NextOccurrenceAfter(afterValue)).Where(o => o != null).Min();
            return result;
        }

        public static DateTime? LastOccurrenceBefore(this IEnumerable<Schedule> schedules, DateTime? before = null)
        {
            if (!schedules.Any())
                return null;

            var beforeValue = before ?? DateTime.Now;
            var scheduleDefinitions = schedules.Select(s => ScheduleDefinitionConverter.Convert(s.ScheduleDefinition));

            var result = scheduleDefinitions.Select(s => s.LastOccurrenceBefore(beforeValue)).Where(o => o != null).Max();
            return result;
        }
    }
}
