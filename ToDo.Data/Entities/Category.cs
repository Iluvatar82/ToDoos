using Core.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDo.Data.Entities
{
    [Table("Category", Schema = "CatalogValues")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        [MinLength(3, ErrorMessageResourceName = "MinLengthMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Bezeichnung { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        public string RGB_A { get; set; }

        public Category()
        {
            Bezeichnung = string.Empty;
            RGB_A = "#ffffffff";
        }


        public override string ToString() => $"{Bezeichnung}, {RGB_A}";
    }
}