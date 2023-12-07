using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDo.Data.ToDoData.Entities.Base;

namespace ToDo.Data.ToDoData.Entities
{
    [Table("Invitation", Schema = "LiveData")]
    public class Invitation : DbEntityBase
    {
        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Required]
        public string InvitationToken { get; set; } = string.Empty;

        [Required]
        public string MailAddress { get; set; } = string.Empty;

        public override string ToString() => $"Invitation from {UserId} for Address {MailAddress}";
    }
}
