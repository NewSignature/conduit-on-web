using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Common;
using Todo.Data;
using Todo.Data.Entities;
using Todo.Web.ViewModels;

namespace Todo.Services.Impl
{
    public class SaveListItemService : ISaveListItemService
    {
        private readonly IContext _context;

        public SaveListItemService(IContext context)
        {
            _context = context;
        }

        public async Task<TodoListItem> CreateListItem(Guid parentListId, CreateListItemViewModel createRequest, Guid currentUserId)
        {
            var targetList = await _context.Lists
                .Include(x => x.Owner)
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == parentListId);

            if (targetList.OwnerId != currentUserId)
                throw new InvalidOperationException("Attempt to add item to list that is not yours");

            var newTodoListItem = new TodoListItem
            {
                AddedOn = DateTime.Now,
                Description = createRequest.Description,
                ItemName = createRequest.ItemName,
                ParentListId = parentListId
            };

            targetList.Items.Add(newTodoListItem);
            await _context.SaveChangesAsync();

            return newTodoListItem;
        }

        public async Task<TodoListItem> UpdateListItem(Guid id, EditTodoListItemViewModel editRequest, Guid currentUserId)
        {
            var listItem = await _context.ListItems
                .Include(x => x.List)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (listItem.List.OwnerId != currentUserId)
                throw new ResourceSharingException();

            if (editRequest.IsComplete && !listItem.CompletedOn.HasValue)
                listItem.CompletedOn = DateTime.Now;

            if (!editRequest.IsComplete)
                listItem.CompletedOn = null;

            listItem.ItemName = editRequest.Name;
            listItem.Description = editRequest.Description;

            await _context.SaveChangesAsync();

            return listItem;
        }
    }
}
