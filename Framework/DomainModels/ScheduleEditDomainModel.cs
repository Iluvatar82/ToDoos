using Framework.DomainModels.Base;

namespace Framework.DomainModels
{
    public class ScheduleEditDomainModel
    {
        public List<ScheduleDomainModel> Schedules { get; set; }
        public List<ScheduleReminderDomainModel> Reminders { get; set; }


        public ScheduleEditDomainModel() { 
            Schedules = new List<ScheduleDomainModel>();
            Reminders = new List<ScheduleReminderDomainModel>();
        }
    }
}
