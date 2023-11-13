using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDo.Data.ToDoData.Entities.Base;

namespace ToDo.Data.ToDoData.Entities
{
    [Table("HangfireJob", Schema = "LiveData")]
    public class HangfireJob : DbEntityBase
    {
        [Required]
        [ForeignKey("ToDoItem")]
        public Guid ToDoItemId { get; set; }

        [Required]
        [ForeignKey("Job")]
        public string JobId { get; set; }
    }
}
