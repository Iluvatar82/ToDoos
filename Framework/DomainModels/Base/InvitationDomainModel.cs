namespace Framework.DomainModels.Base
{
    public class InvitationDomainModel : DomainModelBase
    {
        public Guid UserId { get; set; }

        public string InvitationToken { get; set; } = string.Empty;

        public string MailAddress { get; set; } = string.Empty;
    }
}
