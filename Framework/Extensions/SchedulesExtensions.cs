using Framework.DomainModels.Base;

namespace Framework.Extensions
{
    public static class SchedulesExtensions
    {
        public static DateTime? NextOccurrenceAfter(this IEnumerable<ScheduleDomainModel> schedules, DateTime? after = null)
        {
            if (!schedules.Any())
                return null;

            var afterValue = after ?? DateTime.Now;
            var result = schedules.Select(s => s.ScheduleDefinition.NextOccurrenceAfter(afterValue, s.Start, s.End)).Where(o => o != null).Min();
            return result;
        }

        public static DateTime? LastOccurrenceBefore(this IEnumerable<ScheduleDomainModel> schedules, DateTime? before = null)
        {
            if (!schedules.Any())
                return null;

            var beforeValue = before ?? DateTime.Now;
            var result = schedules.Select(s => s.ScheduleDefinition.LastOccurrenceBefore(beforeValue, s.Start, s.End)).Where(o => o != null).Max();
            return result;
        }

        public static List<DateTime> GetOccurrences(this IEnumerable<ScheduleDomainModel> schedules, DateTime from, DateTime to)
        {
            if (!schedules.Any())
                return new List<DateTime>();

            var result = schedules.SelectMany(s => s.ScheduleDefinition.GetOccurrences(from, to, s.Start, s.End)).ToList();
            return result;
        }
    }
}
