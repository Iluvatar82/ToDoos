namespace Framework.DomainModels.Base
{
    public class UserGroupDomainModel : DomainModelBase
    {
        public string Name { get; set; }

        public Guid GroupId { get; set; }

        public Guid UserId { get; set; }

        public UserGroupDomainModel()
        {
            Name = string.Empty;
        }
    }
}
