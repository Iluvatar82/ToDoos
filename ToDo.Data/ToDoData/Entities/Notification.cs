using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDo.Data.ToDoData.Entities.Base;

namespace ToDo.Data.ToDoData.Entities
{
    [Table("Notification", Schema = "LiveData")]
    [Microsoft.EntityFrameworkCore.Index(nameof(UserId))]
    public class Notification : DbEntityBase
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public DateTime Time { get; set; } = DateTime.Now;

        [Required]
        public string Sender { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        public DateTime? Read { get; set; }
    }
}