using Core.Validation;
using Framework.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDo.Data.Identity;

namespace Framework.Repositories
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

            var allRoles = await dbContext.Roles!.ToListAsync();
            return allRoles;
        }

        public async Task<List<IdentityUser>> GetAllIdentities()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();
            dbContext!.Users.NotNull();

            var allIdentities = await dbContext.Users!.ToListAsync();
            return allIdentities;
        }

        public async Task<IdentityRole?> GetRoleAsync(string roleId)
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();
            dbContext!.Roles.NotNull();

            var role = await dbContext.Roles!.FirstOrDefaultAsync(i => i.Id == roleId);
            return role;
        }

        public async Task<IdentityUser?> GetUserByUsernameAsync(string username)
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();
            dbContext!.Users.NotNull();

            var identity = await dbContext.Users!.FirstOrDefaultAsync(u => u.UserName == username);
            return identity;
        }

        public async Task<List<IdentityUser>> GetUsersByIdAsync(IEnumerable<string> userIds)
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();
            dbContext!.Users.NotNull();

            var allUsersWithRole = await dbContext.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            return allUsersWithRole;
        }

        public async Task<IdentityUser?> GetUserByEmailAsync(string email)
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();
            dbContext!.Users.NotNull();

            var identity = await dbContext.Users!.FirstOrDefaultAsync(u => u.Email == email);
            return identity;
        }

        public async Task ApplyRoleToUserAsync(string roleId, string userId)
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
