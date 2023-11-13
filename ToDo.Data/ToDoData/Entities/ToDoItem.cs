using Core.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDo.Data.ToDoData.Entities.Base;

namespace ToDo.Data.ToDoData.Entities
{
    [Table("ToDoItem", Schema = "LiveData")]
    public class ToDoItem : DbEntityBase
    {
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        [MinLength(3, ErrorMessageResourceName = "MinLengthMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Bezeichnung { get; set; }

        [ForeignKey("Category")]
        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; }

        [Required]
        public int Order { get; set; }

        [ForeignKey("Parent")]
        public Guid? ParentId { get; set; }
        public ToDoItem? Parent { get; set; }

        [InverseProperty(nameof(Parent))]
        public List<ToDoItem> Children { get; set; }

        [ForeignKey("List")]
        public Guid ListId { get; set; }

        public ICollection<Schedule> Schedules { get; set; }

        public ICollection<ScheduleReminder> Reminders { get; set; }

        public DateTime? Done { get; set; }

        public bool IsActive { get; set; }


        public ToDoItem()
        {
            IsActive = true;
            Bezeichnung = string.Empty;
            Children = new List<ToDoItem>();
            Schedules = new List<Schedule>();
            Reminders = new List<ScheduleReminder>();
        }
    }
}
