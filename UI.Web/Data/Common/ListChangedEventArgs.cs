using Framework.DomainModels;

namespace UI.Web.Data.Common
{
    public class ListChangedEventArgs
    {
        public ListChangeType ChangeType { get; set; }
        public ToDoItemDomainModel Item { get; set; }

        public ListChangedEventArgs(ListChangeType type, ToDoItemDomainModel item)
        {
            ChangeType = type;
            Item = item;
        }

        public override string ToString() => $"{ChangeType}: {Item}";
    }
}
