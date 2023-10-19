using Core.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDo.Data.ToDoData.Entities
{
    [Table("UserGroup", Schema = "LiveValues")]
    public class UserGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        [MinLength(3, ErrorMessageResourceName = "MinLengthMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Name { get; set; }

        [Required]
        public Guid GroupId { get; set; }

        [Required]
        public Guid UserId { get; set; }


        public UserGroup()
        {
            Name = string.Empty;
        }


        public override string ToString() => $"Gruppe {Name}, GrupenId: {GroupId}, UserId: {UserId}";
    }
}
