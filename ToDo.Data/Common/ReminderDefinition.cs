using ToDo.Data.Common.Converter;
using ToDo.Data.Common.Enums;

namespace ToDo.Data.Common
{
    public class ReminderDefinition
    {
        public static string Default = $"30 m";


        public decimal Value { get; set; }
        public ScheduleTimeUnit Unit { get; set; }


        public override string ToString() => $"{Value:F2}{TimeUnitConverter.Convert(Unit)}";
    }
}
