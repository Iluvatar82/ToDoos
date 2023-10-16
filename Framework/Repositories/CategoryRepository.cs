using Core.Validation;
using Framework.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using ToDo.Data;
using ToDo.Data.Entities;


namespace Framework.Repositories
{
    public class CategoryRepository : RepositoryBase<ToDoDBContext>
    {
        public CategoryRepository(IDbContextFactory<ToDoDBContext> dbContextFactory)
            :base(dbContextFactory)
        {
        }


        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();
            dbContext!.ToDoItems.NotNull();

            var allCategories = dbContext.Categories.ToList();
            return allCategories;
        }
    }
}