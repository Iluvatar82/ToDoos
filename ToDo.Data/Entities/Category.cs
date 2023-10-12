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

        [Required]
        [MinLength(3)]
        public string Bezeichnung { get; set; }

        [Required]
        public string RGB_A { get; set; }

        public Category()
        {
            Bezeichnung = string.Empty;
            RGB_A = "#ffffffff";
        }


        public override string ToString() => $"{Bezeichnung}, {RGB_A}";
    }
}