using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDo.Data.ToDoData.Entities
{
    [Table("Reminder", Schema = "LiveData")]
    public class ScheduleReminder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("ToDoItem")]
        public Guid ToDoItemId { get; set; }

        [Required]
        public string Definition { get; set; } = string.Empty;
    }
}
