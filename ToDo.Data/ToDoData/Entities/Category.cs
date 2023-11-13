using Core.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDo.Data.ToDoData.Entities
{
    [Table("Category", Schema = "CatalogValues")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public Guid? ListId { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        [MinLength(3, ErrorMessageResourceName = "MinLengthMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Bezeichnung { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        public string RGB_A { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Icon { get; set; }


        public Category()
        {
            Bezeichnung = string.Empty;
            RGB_A = "#ffffffff";
            Icon = string.Empty;
        }


        public override string ToString() => $"{Bezeichnung}, {RGB_A}";
    }
}