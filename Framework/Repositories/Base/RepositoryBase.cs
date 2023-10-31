using Core.Validation;
using Microsoft.EntityFrameworkCore;

namespace Framework.Repositories.Base
{
    public abstract class RepositoryBase<TContext> where TContext : DbContext
    {
        protected IDbContextFactory<TContext> dbContextFactory { get; set; }


        protected RepositoryBase(IDbContextFactory<TContext> contextFactory)
        {
            dbContextFactory = contextFactory;
        }


        public async Task<T?> GetAsync<T>(Guid id) where T : class
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();

            return await dbContext.FindAsync<T>(id);
        }

        public async Task<T?> GetAsync<T>(string id) where T : class
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();

            return await dbContext.FindAsync<T>(id);
        }

        public async Task AddAndSaveAsync<T>(T item) where T : class
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();

            await dbContext.AddAsync(item);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAndSaveAsync<T>(params T[] items) where T : class
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();

            dbContext.UpdateRange(items);
            await dbContext.SaveChangesAsync();
        }

        public async Task RemoveAndSaveAsync<T>(params T[] items) where T : class
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();

            dbContext.RemoveRange(items);
            await dbContext.SaveChangesAsync();
        }
    }
}
