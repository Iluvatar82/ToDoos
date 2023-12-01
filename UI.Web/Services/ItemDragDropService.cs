using Core.Validation;
using Framework.Converter;
using Framework.DomainModels;
using Framework.Extensions;
using Framework.Repositories;
using Microsoft.AspNetCore.Components;

namespace UI.Web.Services
{
    public class ItemDragDropService
    {
        private ModelMapper ModelMapper { get; set; }
        private ItemRepository ItemRepository { get; set; }
        private ToDoItemDomainModel? DraggedItem { get; set; }
        private object? DropToElement { get; set; }
        private EventCallback<(ToDoItemDomainModel?, ToDoItemDomainModel)>? DragFinished { get; set; }

        public ItemDragDropService(ModelMapper modelMapper, ItemRepository itemRepository)
        {
            ModelMapper = modelMapper;
            ItemRepository = itemRepository;
        }

        public void Reset()
        {
            DraggedItem = null;
            DraggedItem = null;
        }

        public void TouchSetDropToElement(object parent)
        {
            if (DraggedItem == null)
            {
                DropToElement = null;
                return;
            }

            DropToElement = parent;
        }

        public void HandleDragStart(ToDoItemDomainModel dragItem, EventCallback<(ToDoItemDomainModel?, ToDoItemDomainModel)>? fromAction)
        {
            DraggedItem = dragItem;
            DragFinished = fromAction;
        }

        public async Task HandleDropOnList(Guid newListId)
        {
            var item = DraggedItem;
            item.NotNull();

            await HandleDraggedFrom(item!.Parent, item, DragFinished, null);

            item.Parent = null;
            item.ParentId = null;

            var allItems = item.SelfAndAllDescendents;
            foreach (var itemFromAll in allItems)
                itemFromAll.ListId = newListId;

            await ItemRepository.UpdateAndSaveAsync(ModelMapper.MapToArray(allItems));

            DraggedItem = null;
        }

        public async Task HandleDrop(ToDoItemDomainModel? newParent,
            EventCallback<(ToDoItemDomainModel?, ToDoItemDomainModel)> onDraggedFrom, EventCallback<(ToDoItemDomainModel?, ToDoItemDomainModel)> onDraggedTo,
            Action updateAction)
        {
            var item = DraggedItem;
            item.NotNull();

            var parent = newParent;
            if (DropToElement != null && DropToElement is ToDoItemDomainModel itemModel)
                parent = itemModel;

            if (parent == item || parent == item!.Parent)
                return;

            await HandleDraggedFrom(item.Parent, item, onDraggedFrom, null);

            item.Parent = parent;
            item.ParentId = parent?.Id;

            await ItemRepository.UpdateAndSaveAsync(ModelMapper.Map(item));

            DraggedItem = null;

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