using ToDo.Data.Common.Converter;
using ToDo.Data.Common.Enums;

namespace ToDo.Data.Common
{
    public class ScheduleInterval
    {
        public static string Default = $"i 1 d";

        public decimal Interval { get; set; }
        public ScheduleTimeUnit Unit {get; set; }

        public override string ToString() => $"{Interval} {TimeUnitConverter.Convert(Unit)}";
    }
}
