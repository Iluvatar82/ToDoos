using Core.Validation;
using Framework.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace UI.Web.Data.Repository
{
    public class IdentityRepository : RepositoryBase<ApplicationDbContext>
    {
        public IdentityRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory) : base(dbContextFactory)
        {
        }

        public async Task<List<IdentityRole>> GetAllRoles()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();
            dbContext!.Roles.NotNull();

            var allItems = await dbContext.Roles!.ToListAsync();
            return allItems;
        }

        public async Task<IdentityRole?> GetRoleCompleteAsync(string roleId)
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();
            dbContext!.Roles.NotNull();

            var item = await dbContext.Roles!.FirstOrDefaultAsync(i => i.Id == roleId);
            return item;
        }

        public async Task<IdentityUser?> GetUserByEmailAsync(string email)
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();
            dbContext!.Users.NotNull();

            var item = await dbContext.Users!.FirstOrDefaultAsync(u => u.Email == email);
            return item;
        }

        public async Task ApplyRoleToUser(string roleId, string userId)
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();

            var newUserRole = new IdentityUserRole<string> { RoleId = roleId, UserId = userId };

            await dbContext.AddAsync(newUserRole);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<IdentityUser>> GetUsersWithRoleAsync(string roleId)
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();
            dbContext!.Roles.NotNull();

            var allUserRolesWithRoleId = dbContext.UserRoles.Where(ur => ur.RoleId == roleId);
            var allUsersWithRole = await dbContext.Users.Where(u => allUserRolesWithRoleId.Any(ur => ur.UserId == u.Id)).ToListAsync();
            return allUsersWithRole;
        }
    }
}
