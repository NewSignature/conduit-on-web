using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Data.Entities;

namespace Todo.Services
{
    public interface IDeleteService
    {
        Task DeleteList(Guid listId, Guid currentUserId);
        Task<TodoListItem> DeleteListItem(Guid listItemId, Guid currentUserId);
    }
}
