using Framework.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using ToDo.Data.ToDoData;

namespace Framework.Repositories
{
    public class ListRepository : RepositoryBase<ToDoDBContext>
    {
        public ListRepository(IDbContextFactory<ToDoDBContext> contextFactory)
            : base(contextFactory)
        {
        }
    }
}
