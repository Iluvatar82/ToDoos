using Framework.DomainModels.Base;
using Framework.Extensions;

namespace Framework.DomainModels
{
    public class ToDoItemDomainModel : DomainModelBase
    {
        public string Bezeichnung { get; set; }

        public Guid? CategoryId { get; set; }
        public CategoryDomainModel? Category { get; set; }

        public int Order { get; set; }

        public Guid? ParentId { get; set; }
        public ToDoItemDomainModel? Parent { get; set; }

        public List<ToDoItemDomainModel> Children { get; set; }

        public Guid ListId { get; set; }

        public ICollection<ScheduleDomainModel> Schedules { get; set; }

        public ICollection<ScheduleReminderDomainModel> Reminders { get; set; }

        public DateTime? Done { get; set; }

        public bool IsActive { get; set; }

        public bool VisuallyDeactivated { get; set; }

        public DateTime? NextOrLastOccurrence => Schedules?.NextOccurrenceAfter(DateTime.Now) ?? Schedules?.LastOccurrenceBefore(DateTime.Now);

        public List<DateTime> DefaultOccurences => Occurrences(DateTime.Today.AddDays(-7), DateTime.Today.AddDays(7));

        public List<DateTime> Occurrences(DateTime from, DateTime to) => Schedules?.GetOccurrences(from, to) ?? new List<DateTime>();


        public IEnumerable<ToDoItemDomainModel> SelfAndAllDescendents
        {
            get
            {
                yield return this;
                foreach (var descendent in AllDescendents)
                    yield return descendent;
            }
        }
        public IEnumerable<ToDoItemDomainModel> AllDescendents
        {
            get
            {
                foreach (var child in Children)
                {
                    yield return child;
                    foreach (var descentent in child.AllDescendents)
                        yield return descentent;
                }
            }
        }


        public ToDoItemDomainModel()
        {
            Bezeichnung = string.Empty;
            Children = new List<ToDoItemDomainModel>();
            Schedules = new List<ScheduleDomainModel>();
            Reminders = new List<ScheduleReminderDomainModel>();
        }

        public static ToDoItemDomainModel Create(Guid? parentId, Guid? categoryId, Guid listId)
        {
            var result = new ToDoItemDomainModel
            {
                ParentId = parentId,
                CategoryId = categoryId,
                ListId = listId,
                IsActive = true
            };

            result.Set("OriginalOrder", 0);
            return result;
        }


        public override string ToString() => $"{Bezeichnung}, Category: {Category}, Deadline: {NextOrLastOccurrence?.ToShortDateString() ?? "-"}";
    }
}