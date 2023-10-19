using ToDo.Data.ToDoData.Entities;

namespace UI.Web.Data.Extensions
{
    public static class DataExtensions
    {
        public static string GetToDoListTypeIcon(this ToDoList list) => list.IsUserList ? "person" : "people";
        public static string GetListUrl(this ToDoList list) => $"list/{list.Id}";
        public static string GetGroupUrl(this UserGroup group) => $"group/{group.GroupId}";
    }
}
