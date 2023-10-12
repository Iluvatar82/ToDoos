using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDo.Data.Entities
{
    [Table("ToDoItem", Schema = "LiveData")]
    public class ToDoItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MinLength(3)]
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

        public DateTime? Deadline { get; set; }

        public DateTime? Done { get; set; }


        public ToDoItem()
        {
            Bezeichnung = string.Empty;
            Children = new List<ToDoItem>();
        }


        public override string ToString() => $"{Bezeichnung}, Category: {Category}, Deadline: {Deadline?.ToShortDateString() ?? "-"}, Order: {Order}";
    }
}
