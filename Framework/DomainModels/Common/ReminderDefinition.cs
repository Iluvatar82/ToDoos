using Framework.DomainModels.Common.Enums;

namespace Framework.DomainModels.Common
{
    public class ReminderDefinition
    {
        public static string Default = $"30 m";


        public decimal Value { get; set; }
        public ScheduleTimeUnit Unit { get; set; }
    }
}
