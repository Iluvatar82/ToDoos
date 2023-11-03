using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDo.Data.ToDoData.Entities
{
    [Table("Schedule", Schema = "LiveData")]
    public class Schedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("ToDoItem")]
        public Guid ItemId { get; set; }

        public DateTime? Start { get; set; }
        
        public DateTime? End { get; set; }

        [Required]
        public string ScheduleDefinition { get; set; } = string.Empty;
    }
}
