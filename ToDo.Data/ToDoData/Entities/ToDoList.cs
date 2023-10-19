using Core.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDo.Data.ToDoData.Entities
{
    [Table("ToDoList", Schema = "LiveValues")]
    public class ToDoList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        [MinLength(3, ErrorMessageResourceName = "MinLengthMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Name { get; set; }

        public Guid? UserId { get; set; }

        public Guid? GroupId { get; set; }

        [NotMapped]
        public bool IsUserList => UserId != null;


        public ToDoList()
        {
            Name = string.Empty;
        }

        public override string ToString() => $"{Name}, ToDoList of{(UserId != null ? " User " + UserId : null)}";
    }
}
