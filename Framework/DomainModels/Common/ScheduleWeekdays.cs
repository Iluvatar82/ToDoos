namespace Framework.DomainModels.Common
{
    public class ScheduleWeekdays
    {
        public static string Default = $"w {string.Join(", ", Enumerable.Repeat(1, 7))}";


        private List<bool> _days;
        public List<bool> Days { get => _days; set => _days = value; }


        public bool Montag { get => _days[0]; set => _days[0] = value; }
        public bool Dienstag { get => _days[1]; set => _days[1] = value; }
        public bool Mittwoch { get => _days[2]; set => _days[2] = value; }
        public bool Donnerstag { get => _days[3]; set => _days[3] = value; }
        public bool Freitag { get => _days[4]; set => _days[4] = value; }
        public bool Samstag { get => _days[5]; set => _days[5] = value; }
        public bool Sonntag { get => _days[6]; set => _days[6] = value; }
        public TimeOnly Time { get; set; }


        public ScheduleWeekdays()
        {
            _days = Enumerable.Repeat(true, 7).ToList();
            Time = new TimeOnly();
        }


        public override string ToString()
        {
            var result = string.Empty;
            var activeDays = new List<string>
            {
                Montag ? "1" : "0",
                Dienstag ? "1" : "0",
                Mittwoch ? "1" : "0",
                Donnerstag ? "1" : "0",
                Freitag ? "1" : "0",
                Samstag ? "1" : "0",
                Sonntag ? "1" : "0"
            };

            result += string.Join(", ", activeDays);

            if (Time != default)
                result += $" {Time.ToShortTimeString()}";

            return result;
        }
    }
}
