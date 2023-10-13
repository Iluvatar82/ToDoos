using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDo.Data.Entities
{
    [Table("UserToDoAssignment", Schema = "LiveData")]
    public class UserToDoAssignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("Item")]
        public Guid ToDoItemId { get; set; }
        public ToDoItem Item { get; set; }

        public Guid UserId { get; set; }


        public UserToDoAssignment()
        {
            Item = new ToDoItem();
        }
    }
}
