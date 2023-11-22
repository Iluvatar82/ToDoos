using Framework.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using ToDo.Data.ToDoData;
using ToDo.Data.ToDoData.Entities;

namespace Framework.Repositories
{
    public class ScheduleRepository : RepositoryBase<ToDoDBContext, Schedule>
    {
        public ScheduleRepository(IDbContextFactory<ToDoDBContext> contextFactory)
            : base(contextFactory)
        {
        }
    }
}
