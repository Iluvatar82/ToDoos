namespace Framework.DomainModels.Base
{
    public class ToDoListDomainModel : DomainModelBase
    {
        public string Name { get; set; }

        public Guid? UserId { get; set; }

        public Guid? GroupId { get; set; }

        public bool IsUserList => UserId != null;

        public ToDoListDomainModel()
        {
            Name = string.Empty;
        }
    }
}
