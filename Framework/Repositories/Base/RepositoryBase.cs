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

        public async Task UpdateAndSaveAsync<T>(T item) where T : class
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();

            var result = dbContext.Update(item);
            await dbContext.SaveChangesAsync();
        }

        public async Task AddAndSaveAsync<T>(T item) where T : class
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();

            await dbContext.AddAsync(item);
            await dbContext.SaveChangesAsync();
        }
    }
}
