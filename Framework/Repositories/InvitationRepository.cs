using Framework.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using ToDo.Data.ToDoData;
using ToDo.Data.ToDoData.Entities;

namespace Framework.Repositories
{
    public class InvitationRepository : RepositoryBase<ToDoDBContext, Invitation>
    {
        public InvitationRepository(IDbContextFactory<ToDoDBContext> contextFactory) : base(contextFactory)
        {
        }
    }
}
