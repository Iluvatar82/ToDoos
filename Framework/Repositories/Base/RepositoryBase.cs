using Core.Validation;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
            dbContext.Database.NotNull();
            dbContext.Satisfies((c) => dbContext.Database.CanConnectAsync().Result);

            return await dbContext.FindAsync<T>(id);
        }

        public async Task<T?> GetAsync<T>(string id) where T : class
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            dbContext.NotNull();
            dbContext.Database.NotNull();
            dbContext.Satisfies((c) => dbContext.Database.CanConnectAsync().Result);

            return await dbContext.FindAsync<T>(id);
        }

        public async Task AddAndSaveAsync<T>(T item) where T : class
        {
            try
            {
                dbContextFactory.NotNull();

                using var dbContext = await dbContextFactory.CreateDbContextAsync();
                dbContext.NotNull();
                dbContext.Database.NotNull();
                dbContext.Satisfies((c) => dbContext.Database.CanConnectAsync().Result);

                await dbContext.AddAsync(item);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Error");
            }
        }

        public async Task UpdateAndSaveAsync<T>(params T[] items) where T : class
        {
            try
            {
                dbContextFactory.NotNull();

                using var dbContext = await dbContextFactory.CreateDbContextAsync();
                dbContext.NotNull();
                dbContext.Database.NotNull();
                dbContext.Satisfies((c) => dbContext.Database.CanConnectAsync().Result);

                dbContext.UpdateRange(items);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString(), "Error");
            }
        }

        public async Task RemoveAndSaveAsync<T>(params T[] items) where T : class
        {
            try
            {
                dbContextFactory.NotNull();

                using var dbContext = await dbContextFactory.CreateDbContextAsync();
                dbContext.NotNull();
                dbContext.Database.NotNull();
                dbContext.Satisfies((c) => dbContext.Database.CanConnectAsync().Result);

                dbContext.RemoveRange(items);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString(), "Error");
            }
        }
    }
}
