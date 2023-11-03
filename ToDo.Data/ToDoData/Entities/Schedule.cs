using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDo.Data.Common;
using ToDo.Data.Common.Converter;

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
        public Guid ToDoItemId { get; set; }

        public DateTime? Start { get; set; }
        
        public DateTime? End { get; set; }

        [Required]
        public string ScheduleDefinition { get; private set; } = string.Empty;

        [NotMapped]
        public ScheduleDefinition Definition { get => ScheduleDefinitionConverter.Convert(ScheduleDefinition); set => ScheduleDefinition = ScheduleDefinitionConverter.Convert(value); }
    }
}
