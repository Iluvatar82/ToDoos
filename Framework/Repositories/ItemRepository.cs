using Core.Validation;
using Framework.Extensions;
using Framework.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using ToDo.Data;
using ToDo.Data.Entities;


namespace Framework.Repositories
{
    public class ItemRepository : RepositoryBase<ToDoDBContext>
    {
        public ItemRepository(IDbContextFactory<ToDoDBContext> dbContextFactory)
            :base(dbContextFactory)
        {
        }


        public async Task<List<ToDoItem>> GetAllItemsCompleteAsync(Guid userId, bool isActive = true)
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();
            dbContext!.ToDoItems.NotNull();

            var allItems = dbContext.ToDoItems!.Where(i => i.UserAssignments.Any(a => a.UserId == userId) && i.IsActive == isActive).IncludeAll().ToList();
            return allItems;
        }

        public async Task<ToDoItem?> GetItemCompleteAsync(Guid itemId)
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();
            dbContext!.ToDoItems.NotNull();

            var item = await dbContext.ToDoItems!.IncludeAll().FirstOrDefaultAsync(i => i.Id == itemId);
            return item;
        }
    }
}