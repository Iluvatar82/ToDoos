using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDo.Data.ToDoData.Entities.Base;

namespace ToDo.Data.ToDoData.Entities
{
    [Table("Setting", Schema = "LiveData")]
    public class Setting : DbEntityBase
    {
        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Required]
        [MinLength(3)]
        public string Key { get; set; } = string.Empty;

        [Required]
        public string Value { get; set; } = string.Empty;


        public override string ToString() => $"Setting {Key} for User {UserId}: {Value}";
    }
}
