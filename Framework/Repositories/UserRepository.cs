using Core.Validation;
using Framework.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using ToDo.Data.ToDoData;
using ToDo.Data.ToDoData.Entities;

namespace Framework.Repositories
{
    public class UserRepository : RepositoryBase<ToDoDBContext>
    {
        public UserRepository(IDbContextFactory<ToDoDBContext> contextFactory)
            : base(contextFactory)
        {
        }


        public async Task<List<ToDoList>> GetAllListsAsync(Guid userId)
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();
            dbContext!.ToDoLists.NotNull();

            var allMyGroups = dbContext.Groups.Where(g => g.UserId == userId);
            var lists = await dbContext.ToDoLists!.Where(l => l.UserId == userId || allMyGroups.Any(g => g.GroupId == l.GroupId)).ToListAsync();
            return lists;
        }

        public async Task<ToDoList> GetListAsync(Guid listId)
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();
            dbContext!.ToDoLists.NotNull();

            var list = await dbContext.ToDoLists.FindAsync(listId);
            return list!;
        }

        public async Task<List<UserGroup>> GetAllGroupsAsync(Guid userId)
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();
            dbContext!.Groups.NotNull();

            var allMyGroups = await dbContext.Groups.Where(g => g.UserId == userId).ToListAsync();
            return allMyGroups;
        }

        public async Task<List<UserGroup>> GetAllUsersForGroupAsync(Guid groupId)
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();
            dbContext!.Groups.NotNull();

            var allMyGroups = await dbContext.Groups.Where(g => g.GroupId == groupId).ToListAsync();
            return allMyGroups;
        }
    }
}
