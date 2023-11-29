using Framework.Services.Base;
using System.Text.RegularExpressions;

namespace Framework.Services
{
    public partial class ItemContentRenderService
    {
        private static Regex WebLinkRegex { get; set; }
        private static Regex EmailLinkRegex { get; set; }
        private static Regex TelLinkRegex { get; set; }


        static ItemContentRenderService()
        {
            WebLinkRegex = new Regex(ServiceResources.ContentRenderService_WebLinkRegex);
            EmailLinkRegex = new Regex(ServiceResources.ContentRenderService_EmailLinkRegex);
            TelLinkRegex = new Regex(ServiceResources.ContentRenderService_TelLinkRegex);
        }


        public static string RenderContent(string raw)
        {
            var result = raw;

            result = result.Replace("<", "&lt;");
            result = result.Replace(">", "&gt;");

            foreach (Match match in WebLinkRegex.Matches(raw))
            {
                var prefix = match.Groups["link"].Value.StartsWith("http") ? string.Empty : "https://";
                var displayValue = match.Groups["name"].Value.Length > 0 ? match.Groups["name"] : match.Groups["link"];
                result = result.Replace(match.Value, $"<a href=\"{prefix}{match.Groups["link"]}\" target=_blank onclick=\"event.stopPropagation()\">{displayValue}</a>");
            }

            foreach (Match match in EmailLinkRegex.Matches(raw))
            {
                var displayValue = match.Groups["name"].Value.Length > 0 ? match.Groups["name"] : match.Groups["email"];
                result = result.Replace(match.Value, $"<a href=\"mailto:{match.Groups["email"]}\" target=_blank onclick=\"event.stopPropagation()\">{displayValue}</a>");
            }

            foreach (Match match in TelLinkRegex.Matches(raw))
            {
                var displayValue = match.Groups["name"].Value.Length > 0 ? match.Groups["name"] : match.Groups["number"];
                result = result.Replace(match.Value, $"<a href=\"tel:{match.Groups["number"]}\" target=_blank onclick=\"event.stopPropagation()\">{displayValue}</a>");
            }

            return $"<div>{result}</div>";
        }

        public static string RenderContentWithoutHtml(string raw)
        {
            var result = raw;

            result = result.Replace("<", "&lt;");
            result = result.Replace(">", "&gt;");

            foreach (Match match in WebLinkRegex.Matches(raw))
                result = result.Replace(match.Value, string.Empty);

            foreach (Match match in EmailLinkRegex.Matches(raw))
                result = result.Replace(match.Value, string.Empty);

            foreach (Match match in TelLinkRegex.Matches(raw))
                result = result.Replace(match.Value, string.Empty);

            return result;
        }
    }
}
