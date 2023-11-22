namespace Framework.DomainModels.Common
{
    public class ScheduleDefinition
    {
        private ScheduleFixed? @fixed;
        private ScheduleWeekdays? weekDays;
        private ScheduleInterval? interval;

        public ScheduleFixed? Fixed
        {
            get => @fixed;
            set
            {
                @fixed = value;
                if (value != null)
                {
                    weekDays = null;
                    interval = null;
                }
            }
        }

        public ScheduleWeekdays? WeekDays
        {
            get => weekDays;
            set
            {
                weekDays = value;
                if (value != null)
                {
                    @fixed = null;
                    interval = null;
                }
            }
        }

        public ScheduleInterval? Interval
        {
            get => interval;
            set
            {
                interval = value;
                if (value != null)
                {
                    @fixed = null;
                    weekDays = null;
                }
            }
        }
    }
}
