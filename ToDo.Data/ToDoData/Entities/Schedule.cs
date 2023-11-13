using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDo.Data.ToDoData.Entities.Base;

namespace ToDo.Data.ToDoData.Entities
{
    [Table("Schedule", Schema = "LiveData")]
    public partial class Schedule : DbEntityBase
    {
        [Required]
        [ForeignKey("ToDoItem")]
        public Guid ToDoItemId { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        [Required]
        public string ScheduleDefinition { get; set; } = string.Empty;
    }
}
