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
            var scheduleDefinitions = schedules.Select((s, i) => (Definition: ScheduleDefinitionConverter.Convert(s.ScheduleDefinition), Index: i));

            var result = scheduleDefinitions.Select(s => s.Definition.NextOccurrenceAfter(afterValue, schedules.ElementAt(s.Index).Start, schedules.ElementAt(s.Index).End)).Where(o => o != null).Min();
            return result;
        }

        public static DateTime? LastOccurrenceBefore(this IEnumerable<Schedule> schedules, DateTime? before = null)
        {
            if (!schedules.Any())
                return null;

            var beforeValue = before ?? DateTime.Now;
            var scheduleDefinitions = schedules.Select((s, i) => (Definition: ScheduleDefinitionConverter.Convert(s.ScheduleDefinition), Index: i));

            var result = scheduleDefinitions.Select(s => s.Definition.LastOccurrenceBefore(beforeValue, schedules.ElementAt(s.Index).Start, schedules.ElementAt(s.Index).End)).Where(o => o != null).Max();
            return result;
        }

        public static List<DateTime> GetOccurrences(this IEnumerable<Schedule> schedules, DateTime from, DateTime to)
        {
            if (!schedules.Any())
                return null;

            var scheduleDefinitions = schedules.Select((s, i) => (Definition: ScheduleDefinitionConverter.Convert(s.ScheduleDefinition), Index: i));

            var result = scheduleDefinitions.SelectMany(s => s.Definition.GetOccurrences(from, to, schedules.ElementAt(s.Index).Start, schedules.ElementAt(s.Index).End)).ToList();
            return result;
        }
    }
}
