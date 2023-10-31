using System.Text.RegularExpressions;

namespace Framework.Services
{
    public partial class ItemContentRenderService
    {
        public string RenderContent(string raw)
        {
            var result = raw;

            var linkRegex = WebLinkRegex();
            foreach (Match match in linkRegex.Matches(raw))
            {
                var prefix = match.Groups["link"].Value.StartsWith("http") ? string.Empty : "https://";
                var displayValue = match.Groups["name"].Value.Length > 0 ? match.Groups["name"] : match.Groups["link"];
                result = result.Replace(match.Value, $"<a href={prefix}{match.Groups["link"]} target=_blank>{displayValue}</a>");
            }

            var emailRegex = EmailLinkRegex();
            foreach (Match match in emailRegex.Matches(raw))
            {
                var displayValue = match.Groups["name"].Value.Length > 0 ? match.Groups["name"] : match.Groups["email"];
                result = result.Replace(match.Value, $"<a href=mailto:{match.Groups["email"]} target=_blank>{displayValue}</a>");
            }

            return $"<div>{result}</div>";
        }


        [GeneratedRegex("(\\[(?<name>.+)\\])?(?<link>(http[s]:\\/\\/|www\\.).+)")]
        private static partial Regex WebLinkRegex();

        [GeneratedRegex("(?<name>\\[[\\w\\s]+\\]){0,1}(?<email>[\\w-\\.]+\\@[\\w-\\.]+\\.[\\w]{2,4})")]
        private static partial Regex EmailLinkRegex();
    }
}
