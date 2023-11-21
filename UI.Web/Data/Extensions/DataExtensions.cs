using Framework.DomainModels.Base;

namespace UI.Web.Data.Extensions
{
    public static class DataExtensions
    {
        public static string GetToDoListTypeIcon(this ToDoListDomainModel list) => list.IsUserList ? "user" : "user-group";

        public static string GetListUrl(this ToDoListDomainModel list) => $"list/{list.Id}";

        public static string GetGroupUrl(this UserGroupDomainModel group) => $"group/{group.GroupId}";
    }
}
