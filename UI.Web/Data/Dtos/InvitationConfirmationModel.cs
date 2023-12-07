using System.ComponentModel.DataAnnotations;

namespace UI.Web.Data.Dtos
{
    public class InvitationConfirmationModel
    {
        public string MailAddress { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "Das {0} muss mindestens {2} und maximal {1} zeichen lang sein.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Ihr Passwort")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Passwort wiederholen")]
        [Compare("Password", ErrorMessage = "Die beiden Eingaben müssen übereinstimmen.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
