using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Data.Entities;
using Todo.Web.ViewModels;

namespace Todo.Services
{
    public interface ISaveListItemService
    {
        Task<TodoListItem> CreateListItem(Guid parentListId, CreateListItemViewModel createRequest, Guid currentUserId);
        Task<TodoListItem> UpdateListItem(Guid id, EditTodoListItemViewModel editRequest, Guid currentUserId);
    }
}
