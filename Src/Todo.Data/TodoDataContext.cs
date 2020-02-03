using System.Data.Entity;
using Todo.Data.Entities;

namespace Todo.Data
{
    internal class TodoDataContext : DbContext, IContext
    {
        public TodoDataContext() : base("TodoConnectionString")
        {

        }

        public IDbSet<User> Users { get; set; }

        public IDbSet<TodoList> Lists { get; set; }

        public IDbSet<TodoListItem> ListItems { get; set; }
    }
}
