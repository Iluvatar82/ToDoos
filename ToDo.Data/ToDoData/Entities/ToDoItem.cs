using Core.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDo.Data.Common.Extensions;

namespace ToDo.Data.ToDoData.Entities
{
    [Table("ToDoItem", Schema = "LiveData")]
    public class ToDoItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

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

        public DateTime? Done { get; set; }

        public bool IsActive { get; set; }

        [NotMapped]
        public bool VisuallyDeactivated { get; set; }

        [NotMapped]
        public List<ToDoItem> AllAncestors
        {
            get
            {
                var allAncestors = Children;
                foreach (var child in Children)
                    AllAncestors.AddRange(child.AllAncestors);

                return allAncestors;
            }
        }

        [NotMapped]
        public DateTime? NextOrLastOccurrence => Schedules?.NextOccurrenceAfter(DateTime.Now) ?? Schedules?.LastOccurrenceBefore(DateTime.Now);

        [NotMapped]
        public List<DateTime> DefaultOccurences => Occurrences(DateTime.Today.AddDays(-7), DateTime.Today.AddDays(7));


        public ToDoItem()
        {
            IsActive = true;
            Bezeichnung = string.Empty;
            Children = new List<ToDoItem>();
            VisuallyDeactivated = false;
            Schedules = new List<Schedule>();
        }


        public List<DateTime> Occurrences(DateTime from, DateTime to) => Schedules?.GetOccurrences(from, to) ?? new List<DateTime>();

        public override string ToString() => $"{Bezeichnung}, Category: {Category}, Deadline: {NextOrLastOccurrence?.ToShortDateString() ?? "-"}";
    }
}
