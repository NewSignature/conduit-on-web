using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Common;
using Todo.Data;
using Todo.Data.Entities;

namespace Todo.Services.Impl
{
    public class DeleteService
    {
        private readonly IContext _context;

        public DeleteService(IContext context)
        {
            _context = context;
        }

        public async Task DeleteList(Guid listId, Guid currentUserId)
        {
            var list = await _context.Lists.FirstOrDefaultAsync(x => x.Id == listId);
            if (list != null)
            {
                if (list.OwnerId != currentUserId)
                    throw new ResourceSharingException();

                _context.Lists.Remove(list);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TodoListItem> DeleteListItem(Guid listItemId, Guid currentUserId)
        {
            var listItem = await _context.ListItems
                .Include(x => x.List).FirstOrDefaultAsync(x => x.Id == listItemId);

            if (listItem != null)
            {
                if (listItem.List.OwnerId != currentUserId)
                    throw new ResourceSharingException();

                _context.ListItems.Remove(listItem);
                await _context.SaveChangesAsync();

                return listItem;
            }

            return null;
        }
    }
}
