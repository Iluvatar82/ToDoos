using Framework.DomainModels;
using Framework.Services;

namespace UI.Web.Services
{
    public class ItemStyleService
    {
        public Dictionary<string, object>? GetRowAttributes(ToDoItemDomainModel model, bool hidden, bool noBackground)
        {
            if (model.Category == null || hidden)
                return null;

            var styleString = noBackground ? "" : $"background: {model.Category!.RGB_A};";
            if (model.Done.HasValue)
            {
                styleString = $"color: {UIColorService.GetDoneTextColor(model.Category!.RGB_A)};";
                return new Dictionary<string, object>
                {
                    { "style", styleString }
                };
            }

            styleString += $"color: {UIColorService.GetTextColor(model.Category!.RGB_A)};";
            return new Dictionary<string, object>
            {
                { "style", styleString }
            };
        }

        public Dictionary<string, object>? GetPaddingLeft(int level)
        {
            return new Dictionary<string, object>
            {
                { "style", $"padding-left: {8 + level * 24}px;" }
            };
        }

        public Dictionary<string, object>? GetCheckWidthMargin()
        {
            return new Dictionary<string, object>
            {
                { "style", $"margin-left: 32px;" }
            };
        }

        public Dictionary<string, object>? GetLeftWidth(int level)
        {
            return new Dictionary<string, object>
            {
                { "style", $"min-width: {level * 24}px;" }
            };
        }
    }
}
