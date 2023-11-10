using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDo.Data.ToDoData.Entities
{
    [Table("HangfireJob", Schema = "LiveData")]
    public class HangfireJob
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("ToDoItem")]
        public Guid ToDoItemId { get; set; }

        [Required]
        [ForeignKey("Job")]
        public string JobId { get; set; }
    }
}
