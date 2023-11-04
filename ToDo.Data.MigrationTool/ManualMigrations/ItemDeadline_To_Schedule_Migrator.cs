using Framework.Repositories;
using ToDo.Data.Common;
using ToDo.Data.ToDoData.Entities;

namespace ToDo.Data.MigrationTool.ManualMigrations
{
    public class ItemDeadline_To_Schedule_Migrator : IMigrator
    {
        private readonly ItemRepository _itemRepository;
        
        public ItemDeadline_To_Schedule_Migrator(
            ItemRepository repository)
        {
            _itemRepository = repository;
        }

        public async Task MigrateAsync()
        {
            var items = await _itemRepository.GetAllAsync<ToDoItem>(i => i.NextOccurence.HasValue);
            foreach (var item in items)
            {
                var newSchedule = new Schedule() { ToDoItemId = item.Id, Definition = new ScheduleDefinition() { Fixed = item.NextOccurence } };
                await _itemRepository.AddAndSaveAsync(newSchedule);
            }
        }
    }
}
