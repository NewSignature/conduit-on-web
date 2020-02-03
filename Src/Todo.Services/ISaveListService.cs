using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Data.Entities;
using Todo.Web.ViewModels;

namespace Todo.Services
{
    public interface ISaveListService
    {
        Task<TodoList> CreateTodoList(string title, Guid ownerId);
        Task<TodoList> UpdateTodoList(Guid listId, TodoListViewModel editReqquest, Guid currentUserId);

    }
}
