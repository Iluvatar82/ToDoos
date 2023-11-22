namespace Framework.DomainModels.Common
{
    public class ScheduleFixed
    {
        public static string Default = $"d {DateTime.Today.AddDays(1)}";

        public DateTime Date { get; set; }


        public static implicit operator ScheduleFixed(DateTime date) => new ScheduleFixed() { Date = date };
        public static implicit operator DateTime?(ScheduleFixed? schedule) => schedule?.Date;
        
        
        public override string ToString() => $"{Date:dd.MM.yyyy HH:mm:ss}";
    }
}
