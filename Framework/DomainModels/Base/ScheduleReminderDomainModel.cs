using ToDo.Data.Common;

namespace Framework.DomainModels.Base
{
    public class ScheduleReminderDomainModel : DomainModelBase
    {
        public Guid ToDoItemId { get; set; }

        public required ReminderDefinition ReminderDefinition { get; set; }
    }
}
