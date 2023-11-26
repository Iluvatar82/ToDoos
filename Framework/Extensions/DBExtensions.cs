using Microsoft.EntityFrameworkCore;
using ToDo.Data.ToDoData.Entities;

namespace Framework.Extensions
{
    public static class DBExtensions
    {
        public static IQueryable<ToDoItem> IncludeAll(this IQueryable<ToDoItem> itemSet)
        {
            return itemSet
                .Include(entity => entity.Category)
                .Include(entity => entity.Parent)
                .Include(entity => entity.Schedules)
                .Include(entity => entity.Reminders)
                .AsSplitQuery();
        }
    }
}