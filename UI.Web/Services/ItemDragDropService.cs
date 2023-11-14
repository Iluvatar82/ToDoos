using AutoMapper;
using Core.Validation;
using Framework.DomainModels;
using Framework.Extensions;
using Framework.Repositories;
using Microsoft.AspNetCore.Components;
using ToDo.Data.ToDoData.Entities;

namespace UI.Web.Services
{
    public class ItemDragDropService
    {
        private IMapper _mapper { get; set; }
        private ItemRepository _itemRepository { get; set; }

        public ItemDragDropService(IMapper mapper, ItemRepository itemRepository)
        {
            _mapper = mapper;
            _itemRepository = itemRepository;
        }


        public void HandleDragStart(Areas.LiveData.ToDoList container, ToDoItemDomainModel dragItem)
        {
            if (container != null)
                container.DraggedToDoItem = dragItem;
        }

        public async Task HandleDrop(Areas.LiveData.ToDoList container, ToDoItemDomainModel? dropItem,
            EventCallback<(ToDoItemDomainModel?, ToDoItemDomainModel)> onDraggedFrom, EventCallback<(ToDoItemDomainModel?, ToDoItemDomainModel)> onDraggedTo,
            Action updateAction)
        {
            var item = container?.DraggedToDoItem;
            item.NotNull();

            if (dropItem == item || dropItem == item!.Parent)
                return;

            await HandleDraggedFrom(item.Parent, item, onDraggedFrom, null);

            item.Parent = dropItem;
            item.ParentId = dropItem?.Id;
            await _itemRepository.UpdateAndSaveAsync(_mapper.Map<ToDoItem>(item));

            if (container != null)
                container.DraggedToDoItem = null;

            await HandleDraggedTo(item.Parent, item, onDraggedTo, null);

            updateAction();
        }

        public async Task HandleDraggedFrom(ToDoItemDomainModel? source, ToDoItemDomainModel item, EventCallback<(ToDoItemDomainModel?, ToDoItemDomainModel)>? onDraggedFrom, Action? updateAction)
        {
            source?.Children.Remove(item);
            if (onDraggedFrom != null)
                await onDraggedFrom.Value.InvokeAsync((source, item));

            updateAction?.Invoke();
        }

        public async Task HandleDraggedTo(ToDoItemDomainModel? destination, ToDoItemDomainModel item, EventCallback<(ToDoItemDomainModel?, ToDoItemDomainModel)>? onDraggedTo, Action? updateAction)
        {
            var collection = destination?.Children ?? new List<ToDoItemDomainModel>();
            if (!collection.Contains(item))
            {
                collection.Add(item);
                collection = collection
                    .OrderBy(item => item.Done.HasValue)
                    .ThenBy(item => item.Order)
                    .ThenByDescending(item => item.NextOrLastOccurrence)
                    .ForEach((dm, index) => dm.Set("OriginalOrder", index))
                .ToList();
            }
            
            if (onDraggedTo != null)
                await onDraggedTo.Value.InvokeAsync((destination, item));

            updateAction?.Invoke();
        }

        public List<ToDoItemDomainModel> HandleDraggedFrom(List<ToDoItemDomainModel> elements, ToDoItemDomainModel? source, ToDoItemDomainModel item)
        {
            if (source != null)
                return elements;

            elements.Remove(item);
            return elements;
        }

        public List<ToDoItemDomainModel> HandleDraggedTo(List<ToDoItemDomainModel> elements, ToDoItemDomainModel? destination, ToDoItemDomainModel item)
        {
            if (destination != null)
                return elements;

            if (!elements.Contains(item))
            {
                elements.Add(item);
                elements = elements
                    .OrderBy(item => item.Done.HasValue)
                    .ThenBy(item => item.Order)
                    .ThenByDescending(item => item.NextOrLastOccurrence)
                    .ForEach((dm, index) => dm.Set("OriginalOrder", index))
                       .ToList();
            }

            return elements;
        }
    }
}