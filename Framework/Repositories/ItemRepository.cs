using Core.Validation;
using Framework.Extensions;
using Framework.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using ToDo.Data.ToDoData;
using ToDo.Data.ToDoData.Entities;

namespace Framework.Repositories
{
    public class ItemRepository : RepositoryBase<ToDoDBContext, ToDoItem>
    {
        public ItemRepository(IDbContextFactory<ToDoDBContext> dbContextFactory)
            :base(dbContextFactory)
        {
        }

        public async Task<List<ToDoItem>> GetAllItemsCompleteAsync(Guid listId, bool showInactive)
        {
            using var dbContext = await DbContextFactory.CreateDbContextAsync();
            dbContext.NotNull();
            dbContext!.ToDoItems.NotNull();

            return await dbContext.ToDoItems!.Where(i => i.ListId == listId && i.InactiveSince.HasValue == showInactive).IncludeAll().ToListAsync();
        }

        public async Task<List<ToDoItem>> GetAllItemsCompleteAsync(Func<ToDoItem, bool> filterFunc)
        {
            using var dbContext = await DbContextFactory.CreateDbContextAsync();
            dbContext.NotNull();
            dbContext!.ToDoItems.NotNull();

            return dbContext.ToDoItems!.IncludeAll().Where(filterFunc).ToList();
        }

        public async Task<ToDoItem?> GetItemCompleteAsync(Guid itemId)
        {
            using var dbContext = await DbContextFactory.CreateDbContextAsync();
            dbContext.NotNull();
            dbContext!.ToDoItems.NotNull();

            var item = await dbContext.ToDoItems!.IncludeAll().FirstOrDefaultAsync(i => i.Id == itemId);
            return item;
        }
    }
}