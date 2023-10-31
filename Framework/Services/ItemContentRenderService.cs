using Framework.Services.Base;
using System.Text.RegularExpressions;

namespace Framework.Services
{
    public partial class ItemContentRenderService
    {
        private static Regex WebLinkRegex { get; set; }
        private static Regex EmailLinkRegex { get; set; }


        static ItemContentRenderService()
        {
            WebLinkRegex = new Regex(ServiceResources.ContentRenderService_WebLinkRegex);
            EmailLinkRegex = new Regex(ServiceResources.ContentRenderService_EmailLinkRegex);
        }


        public static string RenderContent(string raw)
        {
            var result = raw;

            foreach (Match match in WebLinkRegex.Matches(raw))
            {
                var prefix = match.Groups["link"].Value.StartsWith("http") ? string.Empty : "https://";
                var displayValue = match.Groups["name"].Value.Length > 0 ? match.Groups["name"] : match.Groups["link"];
                result = result.Replace(match.Value, $"<a href={prefix}{match.Groups["link"]} target=_blank>{displayValue}</a>");
            }

            foreach (Match match in EmailLinkRegex.Matches(raw))
            {
                var displayValue = match.Groups["name"].Value.Length > 0 ? match.Groups["name"] : match.Groups["email"];
                result = result.Replace(match.Value, $"<a href=mailto:{match.Groups["email"]} target=_blank>{displayValue}</a>");
            }

            return $"<div>{result}</div>";
        }
    }
}
