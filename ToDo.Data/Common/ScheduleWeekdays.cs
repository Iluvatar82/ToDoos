namespace ToDo.Data.Common
{
    public class ScheduleWeekdays
    {
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
            _days = Enumerable.Repeat(false, 7).ToList();
            Time = new TimeOnly();
        }



        public override string ToString()
        {
            var result = string.Empty;
            var activeDays = new List<string>();
            if (Montag)
                activeDays.Add(nameof(Montag));
            
            if (Dienstag)
                activeDays.Add(nameof(Dienstag));
            
            if (Mittwoch)
                activeDays.Add(nameof(Mittwoch));
            
            if (Donnerstag)
                activeDays.Add(nameof(Donnerstag));
            
            if (Freitag)
                activeDays.Add(nameof(Freitag));
            
            if (Samstag)
                activeDays.Add(nameof(Samstag));
            
            if (Sonntag)
                activeDays.Add(nameof(Sonntag));

            result = string.Join(", ", activeDays);

            if (Time != default)
                result += $" - {Time.ToShortTimeString()}";

            return result;
        }
    }
}
