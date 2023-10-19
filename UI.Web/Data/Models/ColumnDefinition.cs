using System.Linq.Expressions;

namespace UI.Web.Data.Models
{
    public class ColumnDefinition<TItem>
    {
        public string Header { get; set; } = string.Empty;
        public Func<TItem, string> Accessor { get; set; } = (_) => string.Empty;
        public Action<TItem, string> Setter { get; set; } = (_, _) => { };
        public string InputType { get; set; } = "text";
        public bool IsEditable { get; set; } = false;
        public Expression<Func<TItem, string>> PropertyExpression { get; set; }
    }
}
