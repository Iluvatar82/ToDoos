using Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace UI.Web.Data.Dtos
{
    public class EmailDto
    {
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        [EmailAddress(ErrorMessageResourceName = "EmailMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Email { get; set; }

        public EmailDto()
        {
            Email = string.Empty;
        }
    }
}
