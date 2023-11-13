using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDo.Data.ToDoData.Entities.Base;

namespace ToDo.Data.ToDoData.Entities
{
    [Table("Reminder", Schema = "LiveData")]
    public class ScheduleReminder : DbEntityBase
    {
        [Required]
        [ForeignKey("ToDoItem")]
        public Guid ToDoItemId { get; set; }

        [Required]
        public string Definition { get; set; } = string.Empty;
    }
}
