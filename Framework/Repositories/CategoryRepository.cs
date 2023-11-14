using Core.Validation;
using Framework.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using ToDo.Data.ToDoData;
using ToDo.Data.ToDoData.Entities;

namespace Framework.Repositories
{
    public class CategoryRepository : RepositoryBase<ToDoDBContext>
    {
        public CategoryRepository(IDbContextFactory<ToDoDBContext> dbContextFactory)
            : base(dbContextFactory)
        {
        }


        public async Task<List<Category>> GetAllCategoriesAsync(Func<Category, bool>? filterFunc = null)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            dbContext.NotNull();
            dbContext!.ToDoItems.NotNull();

            var allCategories = dbContext.Categories.AsEnumerable();
            if (filterFunc != null)
                allCategories = allCategories.Where(filterFunc);

            return allCategories.ToList();
        }
    }
}