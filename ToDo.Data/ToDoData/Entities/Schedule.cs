using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDo.Data.Common.Enums;

namespace ToDo.Data.ToDoData.Entities
{
    [Table("Schedule", Schema = "LiveData")]
    public partial class Schedule
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
        public string ScheduleDefinition { get; set; } = string.Empty;

        [NotMapped]
        public ScheduleType Type
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ScheduleDefinition))
                    return default;

                if (ScheduleDefinition[0] == 'd')
                    return ScheduleType.Fixed;

                if (ScheduleDefinition[0] == 'w')
                    return ScheduleType.WeekDays;

                if (ScheduleDefinition[0] == 'i')
                    return ScheduleType.Interval;

                return default;
            }
        }
    }
}
