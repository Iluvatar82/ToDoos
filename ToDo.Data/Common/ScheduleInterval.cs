using ToDo.Data.Common.Enums;

namespace ToDo.Data.Common
{
    public class ScheduleInterval
    {
        public static string Default = $"i 1 d";

        public decimal Interval { get; set; }
        public ScheduleTimeUnit Unit {get; set; }


        public override string ToString()
        {
            var unit = Unit switch
            {
                ScheduleTimeUnit.Minute => "m",
                ScheduleTimeUnit.Hour => "h",
                ScheduleTimeUnit.Day => "d",
                ScheduleTimeUnit.Week => "w",
                ScheduleTimeUnit.Month => "M",
                ScheduleTimeUnit.Year => "y",
                _ => "d",
            };

            return $"{Interval} {unit}";
        }
    }
}
