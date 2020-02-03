using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Data.Entities;

namespace Todo.Data
{
    public interface IContext
    {
        IDbSet<User> Users { get; }

        IDbSet<TodoList> Lists { get; }

        IDbSet<TodoListItem> ListItems { get; }

        Task<int> SaveChangesAsync();
    }
}
