namespace ToDo.Data.Common
{
    public class ScheduleInterval
    {
        public static string Default = $"i 1";

        public decimal Interval { get; set; }


        public static implicit operator ScheduleInterval(decimal interval) => new ScheduleInterval() { Interval = interval };
        public static implicit operator decimal(ScheduleInterval schedule) => schedule.Interval;

        public override string ToString() => Interval.ToString();
    }
}
