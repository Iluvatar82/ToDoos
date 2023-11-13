using AutoMapper;
using Framework.Converter;
using Framework.DomainModels;
using Framework.Repositories;
using ToDo.Data.Common;
using ToDo.Data.ToDoData.Entities;

namespace ToDo.Data.MigrationTool.ManualMigrations
{
    public class ItemDeadline_To_Schedule_Migrator : IMigrator
    {
        private readonly ItemRepository _itemRepository;
        public IMapper Mapper { get; set; }


        public ItemDeadline_To_Schedule_Migrator(
            ItemRepository repository, Mapper mapper)
        {
            _itemRepository = repository;
            Mapper = mapper;
        }


        public async Task MigrateAsync()
        {
            var items = await _itemRepository.GetAllAsync<ToDoItem>();
            var mappeItems = Mapper.Map<List<ToDoItem>, List<ToDoItemDomainModel>>(items);
            foreach (var item in mappeItems.Where(i => i.NextOrLastOccurrence.HasValue))
            {
                var newSchedule = new Schedule() { ToDoItemId = item.Id, ScheduleDefinition = new ScheduleDefinitionConverter(null).Convert(new ScheduleDefinition() { Fixed = item.NextOrLastOccurrence }, null) };
                await _itemRepository.AddAndSaveAsync(newSchedule);
            }
        }
    }
}
