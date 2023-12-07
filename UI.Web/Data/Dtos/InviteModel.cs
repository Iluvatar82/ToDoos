using Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace UI.Web.Data.Dtos
{
    public class InviteModel
    {
        public string InvitationFrom { get; set; } = string.Empty;

        [Display(Name = "Mail Adresse")]
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        [EmailAddress(ErrorMessageResourceName = "EmailMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        public string MailAddress { get; set; } = string.Empty;
    }
}
