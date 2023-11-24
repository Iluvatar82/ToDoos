using Framework.Repositories;
using ServiceJobs.Base;

namespace ServiceJobs
{
    public class DatabaseCleanupJob : IJob
    {
        private readonly ItemRepository _repository;

        public DatabaseCleanupJob(ItemRepository repository)
        {
            _repository = repository;
        }

        public async Task Execute()
        {
            var allOldItems = await _repository.GetAllItemsCompleteAsync(i => i.InactiveSince != null && i.InactiveSince < DateTime.Now.AddDays(-7));
            
            var allSchedules = allOldItems.SelectMany(i => i.Schedules).ToArray();
            var allReminders = allOldItems.SelectMany(i => i.Reminders).ToArray();

            await _repository.RemoveAndSaveAsync(allSchedules);
            await _repository.RemoveAndSaveAsync(allReminders);

            await _repository.RemoveAndSaveAsync(allOldItems.ToArray());
        }
    }
}
