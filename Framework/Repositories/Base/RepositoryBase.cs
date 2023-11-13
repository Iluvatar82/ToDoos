using Core.Validation;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ToDo.Data.ToDoData.Entities.Base;

namespace Framework.Repositories.Base
{
    public abstract class RepositoryBase<TContext> where TContext : DbContext
    {
        protected IDbContextFactory<TContext> dbContextFactory { get; set; }


        protected RepositoryBase(IDbContextFactory<TContext> contextFactory)
        {
            dbContextFactory = contextFactory;
        }


        public async Task<List<T>> GetAllAsync<T>() where T : class
        {

            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            dbContext.NotNull();
            dbContext.Database.NotNull();
            dbContext.Satisfies((c) => dbContext.Database.CanConnectAsync().Result);

            return await dbContext.Set<T>().ToListAsync();
        }

        public async Task<List<T>> GetAllAsync<T>(Func<T, bool> filterFunc) where T : class
        {

            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            dbContext.NotNull();
            dbContext.Database.NotNull();
            dbContext.Satisfies((c) => dbContext.Database.CanConnectAsync().Result);

            return dbContext.Set<T>().Where(filterFunc).ToList();
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

        public async Task AddAndSaveAsync<T>(params T[] items) where T : class
        {
            try
            {
                dbContextFactory.NotNull();

                using var dbContext = await dbContextFactory.CreateDbContextAsync();
                dbContext.NotNull();
                dbContext.Database.NotNull();
                dbContext.Satisfies((c) => dbContext.Database.CanConnectAsync().Result);

                await dbContext.AddRangeAsync(items);
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

        public async Task AddOrUpdateAndSaveAsync<T>(params T[] items) where T : DbEntityBase
        {
            try
            {
                dbContextFactory.NotNull();

                using var dbContext = await dbContextFactory.CreateDbContextAsync();
                dbContext.NotNull();
                dbContext.Database.NotNull();
                dbContext.Satisfies((c) => dbContext.Database.CanConnectAsync().Result);

                var newItems = items.Where(i => i.Id == Guid.Empty).ToList();
                var updateItems = items.Where(i => !newItems.Contains(i)).ToList();

                await dbContext.AddRangeAsync(newItems);
                dbContext.UpdateRange(updateItems);
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
