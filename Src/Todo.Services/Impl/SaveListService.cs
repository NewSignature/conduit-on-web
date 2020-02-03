using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Todo.Common;
using Todo.Data;
using Todo.Data.Entities;
using Todo.Web.ViewModels;

namespace Todo.Services.Impl
{
    public class SaveListService
    {
        private readonly IContext _context;

        public SaveListService(IContext context)
        {
            _context = context;
        }

        public async Task<TodoList> CreateTodoList(string title, Guid ownerId)
        {
            var newList = new TodoList
            {
                Title = title,
                CreatedOn = DateTime.Now,
                OwnerId = ownerId
            };

            _context.Lists.Add(newList);
            await _context.SaveChangesAsync();

            return newList;
        }

        public async Task<TodoList> UpdateTodoList(Guid listId, TodoListViewModel editReqquest, Guid currentUserId)
        {
            var targetList = await _context.Lists.FirstOrDefaultAsync(x => x.Id == listId);
            if (targetList.OwnerId != currentUserId)
                throw new ResourceSharingException();

            targetList.Title = editReqquest.Title;
            await _context.SaveChangesAsync();

            return targetList;
        }
    }
}
