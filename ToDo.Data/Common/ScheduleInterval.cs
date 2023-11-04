namespace ToDo.Data.Common
{
    public class ScheduleInterval
    {
        public decimal Interval { get; set; }


        public static implicit operator ScheduleInterval(decimal interval) => new ScheduleInterval() { Interval = interval };
        public static implicit operator decimal(ScheduleInterval schedule) => schedule.Interval;

        public override string ToString() => $"Intervall: {Interval} Tage";
    }
}
