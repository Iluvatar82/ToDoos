namespace Framework.DomainModels.Base
{
    public class HangfireJobDomainModel : DomainModelBase
    {
        public Guid ToDoItemId { get; set; }

        public string JobId { get; set; } = string.Empty;
    }
}
