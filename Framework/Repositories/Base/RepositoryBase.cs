using Core.Validation;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ToDo.Data.ToDoData.Entities.Base;

namespace Framework.Repositories.Base
{
    public abstract class RepositoryBase<TContext, TEntity> where TContext : DbContext where TEntity : class
    {
        protected IDbContextFactory<TContext> dbContextFactory { get; set; }


        protected RepositoryBase(IDbContextFactory<TContext> contextFactory)
        {
            dbContextFactory = contextFactory;
        }


        public async Task<List<TEntity>> GetAllAsync()
        {
            using var dbContext =  await dbContextFactory.CreateDbContextAsync();
            dbContext.NotNull();
            dbContext.Database.NotNull();
            dbContext.Satisfies((c) => dbContext.Database.CanConnectAsync().Result);

            return await dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(Func<TEntity, bool> filterFunc)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            dbContext.NotNull();
            dbContext.Database.NotNull();
            dbContext.Satisfies((c) => dbContext.Database.CanConnectAsync().Result);

            return dbContext.Set<TEntity>().Where(filterFunc).ToList();
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

        public async Task<TEntity?> GetAsync(Guid id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            dbContext.NotNull();
            dbContext.Database.NotNull();
            dbContext.Satisfies((c) => dbContext.Database.CanConnectAsync().Result);

            return await dbContext.FindAsync<TEntity>(id);
        }

        public async Task<TEntity?> GetAsync(string id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            dbContext.NotNull();
            dbContext.Database.NotNull();
            dbContext.Satisfies((c) => dbContext.Database.CanConnectAsync().Result);

            return await dbContext.FindAsync<TEntity>(id);
        }
        public async Task<T?> GetAsync<T>(Guid id) where T : class
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            dbContext.NotNull();
            dbContext.Database.NotNull();
            dbContext.Satisfies((c) => dbContext.Database.CanConnectAsync().Result);

            return await dbContext.FindAsync<T>(id);
        }

        public async Task<T?> GetAsync<T>(string id) where T : class
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            dbContext.NotNull();
            dbContext.Database.NotNull();
            dbContext.Satisfies((c) => dbContext.Database.CanConnectAsync().Result);

            return await dbContext.FindAsync<T>(id);
        }

        public async Task<Guid> AddAndSaveAsync<T>(T item) where T : class
        {
            try
            {
                using var dbContext = await dbContextFactory.CreateDbContextAsync();
                dbContext.NotNull();
                dbContext.Database.NotNull();
                dbContext.Satisfies((c) => dbContext.Database.CanConnectAsync().Result);

                await dbContext.AddAsync(item);
                await dbContext.SaveChangesAsync();

                item.Satisfies(i => i is DbEntityBase);

                return (item as DbEntityBase)!.Id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Error");
                return Guid.Empty;
            }
        }

        public async Task AddAndSaveAsync<T>(params T[] items) where T : class
        {
            try
            {
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

        public async Task AddOrUpdateAndSaveAsync<T>(params T[] items) where T : class
        {
            try
            {
                using var dbContext = await dbContextFactory.CreateDbContextAsync();
                dbContext.NotNull();
                dbContext.Database.NotNull();
                dbContext.Satisfies((c) => dbContext.Database.CanConnectAsync().Result);

                var dbItems = items.Cast<DbEntityBase>().ToList();

                var newItems = dbItems.Where(i => i.Id == Guid.Empty).Cast<T>().ToList();
                var updateItems = dbItems.Where(i => i.Id != Guid.Empty).Cast<T>().ToList();

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
