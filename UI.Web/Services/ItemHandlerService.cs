using Framework.Converter;
using Framework.DomainModels;
using Framework.Extensions;
using Framework.Repositories;
using Microsoft.AspNetCore.Components;

namespace UI.Web.Services
{
    public class ItemHandlerService
    {
        private ModelMapper _modelMapper { get; set; }
        private ItemRepository _itemRepository { get; set; }

        public ItemHandlerService(ModelMapper modelMapper, ItemRepository itemRepository)
        {
            _modelMapper = modelMapper;
            _itemRepository = itemRepository;
        }


        public List<ToDoItemDomainModel> Order(List<ToDoItemDomainModel> elements)
        {
            return elements
                    .OrderBy(item => item.Done.HasValue)
                    .ThenBy(item => item.Order)
                    .ThenByDescending(item => item.NextOrLastOccurrence)
                    .ForEach((dm, i) => dm.Set("OriginalOrder", i))
                .ToList();
        }

        public async Task HandleRemove(List<ToDoItemDomainModel> elements, ToDoItemDomainModel todoItem, EventCallback<ToDoItemDomainModel>? onRemove, Action updateAction)
        {
            elements.Remove(todoItem);
            updateAction.Invoke();

            if (onRemove != null)
                await onRemove.Value.InvokeAsync(todoItem);
        }

        public async Task<List<ToDoItemDomainModel>> HandleNewChange(List<ToDoItemDomainModel> elements, ToDoItemDomainModel? item, Action updateAction)
        {
            if (item != null)
            {
                elements.Add(item);
                elements = Order(elements);
            }

            updateAction.Invoke();
            return await Task.FromResult(elements);
        }

        public async Task ChangeChildOrder(List<ToDoItemDomainModel> elements, ToDoItemDomainModel itemToMove, int change, Action updateAction)
        {
            if (!elements.Contains(itemToMove))
                return;

            var currentIndex = elements.IndexOf(itemToMove);
            elements.Remove(itemToMove);
            var newIndex = Math.Clamp(currentIndex + change, 0, elements.Count);
            elements.Insert(newIndex, itemToMove);

            for (var index = 0; index < elements.Count; index++)
                elements[index].Order = index;

            await _itemRepository.UpdateAndSaveAsync(_modelMapper.MapToArray(elements));
            updateAction.Invoke();
        }

        public async Task<List<ToDoItemDomainModel>> RemoveOrder(List<ToDoItemDomainModel> elements, ToDoItemDomainModel child, Action updateAction)
        {
            if (!elements.Contains(child))
                return elements;

            for (var index = 0; index < elements.Count; index++)
                elements[index].Order = 0;

            elements = elements.OrderBy(i => i.Get<int>("OriginalOrder")).ToList();

            await _itemRepository.UpdateAndSaveAsync(_modelMapper.MapToArray(elements));
            updateAction.Invoke();
            return elements;
        }
    }
}