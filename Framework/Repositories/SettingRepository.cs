using Core.Validation;
using Framework.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using ToDo.Data.ToDoData;
using ToDo.Data.ToDoData.Entities;

namespace Framework.Repositories
{
    public class SettingRepository : RepositoryBase<ToDoDBContext>
    {
        public SettingRepository(IDbContextFactory<ToDoDBContext> contextFactory) : base(contextFactory)
        {
        }


        public async Task<List<Setting>> GetAllSettingsAsync(Guid userId)
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();
            dbContext!.Settings.NotNull();

            return await dbContext.Settings!.Where(s => s.UserId == userId).ToListAsync();
        }

        public async Task<Setting?> GetSettingAsync(Guid userId, string key)
        {
            dbContextFactory.NotNull();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.NotNull();
            dbContext!.Settings.NotNull();

            return await dbContext.Settings!.FirstOrDefaultAsync(s => s.UserId == userId && s.Key == key);
        }

        public Setting Create(Guid userId,  string key, string value) => new Setting() {  UserId = userId, Key = key, Value = value };
    }
}
