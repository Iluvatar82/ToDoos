using Core.Validation;
using Framework.Extensions;
using Framework.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using ToDo.Data.ToDoData;
using ToDo.Data.ToDoData.Entities;

namespace Framework.Repositories
{
    public class ItemRepository : RepositoryBase<ToDoDBContext>
    {
        public ItemRepository(IDbContextFactory<ToDoDBContext> dbContextFactory)
            :base(dbContextFactory)
        {
        }


        public async Task<List<ToDoItem>> GetAllItemsCompleteAsync(Guid listId, bool? isActive = null)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            dbContext.NotNull();
            dbContext!.ToDoItems.NotNull();

            if (isActive == null)
                return await dbContext.ToDoItems!.Where(i => i.ListId == listId).IncludeAll().ToListAsync();
            else
                return await dbContext.ToDoItems!.Where(i => i.ListId == listId && i.IsActive == isActive).IncludeAll().ToListAsync();
        }

        public async Task<ToDoItem?> GetItemCompleteAsync(Guid itemId)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            dbContext.NotNull();
            dbContext!.ToDoItems.NotNull();

            var item = await dbContext.ToDoItems!.IncludeAll().FirstOrDefaultAsync(i => i.Id == itemId);
            return item;
        }
    }
}