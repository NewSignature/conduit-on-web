using System.Data.Entity;
using Todo.Data.Entities;
using Todo.Data.Migrations;

namespace Todo.Data
{
    internal class TodoDataContext : DbContext, IContext
    {
        public TodoDataContext() : base("TodoConnectionString")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<TodoDataContext, Configuration>());
        }

        public IDbSet<User> Users { get; set; }

        public IDbSet<TodoList> Lists { get; set; }

        public IDbSet<TodoListItem> ListItems { get; set; }
    }
}
