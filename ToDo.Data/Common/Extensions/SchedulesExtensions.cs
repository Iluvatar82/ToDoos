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

            var result = scheduleDefinitions.Min(s => s.NextOccurrenceAfter(afterValue));
            return result;
        }
    }
}
