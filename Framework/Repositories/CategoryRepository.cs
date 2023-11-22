using Core.Validation;
using Framework.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using ToDo.Data.ToDoData;
using ToDo.Data.ToDoData.Entities;

namespace Framework.Repositories
{
    public class CategoryRepository : RepositoryBase<ToDoDBContext, Category>
    {
        public CategoryRepository(IDbContextFactory<ToDoDBContext> dbContextFactory)
            : base(dbContextFactory)
        {
        }
    }
}