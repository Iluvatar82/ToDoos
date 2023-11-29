using Core.Validation;
using Framework.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using ToDo.Data.ToDoData;
using ToDo.Data.ToDoData.Entities;

namespace Framework.Repositories
{
    public class NotificationRepository : RepositoryBase<ToDoDBContext, Notification>
    {
        public NotificationRepository(IDbContextFactory<ToDoDBContext> contextFactory)
            : base(contextFactory)
        {
        }

        public async Task<int> GetUnreadCount(Guid userId)
        {
            using var dbContext = await DbContextFactory.CreateDbContextAsync();
            dbContext.NotNull();
            dbContext.Database.NotNull();
            dbContext.Satisfies((c) => dbContext.Database.CanConnectAsync().Result);

            return await dbContext.Notifications.CountAsync(n => n.UserId == userId && n.Read == null);
        }
    }
}
