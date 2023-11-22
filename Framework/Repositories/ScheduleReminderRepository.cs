using Framework.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using ToDo.Data.ToDoData;
using ToDo.Data.ToDoData.Entities;

namespace Framework.Repositories
{
    public class ScheduleReminderRepository : RepositoryBase<ToDoDBContext, ScheduleReminder>
    {
        public ScheduleReminderRepository(IDbContextFactory<ToDoDBContext> contextFactory)
            : base(contextFactory)
        {
        }
    }
}
