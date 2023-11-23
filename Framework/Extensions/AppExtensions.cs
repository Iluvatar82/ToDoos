using Framework.Services.Base;

namespace Framework.Extensions
{
    public static class AppExtensions
    {
        public static string ToListUrl(this Guid listId) => $"{AppResources.Application_Url}/list/{listId}";
        public static string ToScheduleEditUrl(this Guid itemId) => $"{AppResources.Application_Url}/schedule/{itemId}";
    }
}
