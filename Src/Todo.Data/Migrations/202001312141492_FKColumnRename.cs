namespace Todo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FKColumnRename : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TodoListItems", name: "ParentListId", newName: "ListId");
            RenameIndex(table: "dbo.TodoListItems", name: "IX_ParentListId", newName: "IX_ListId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.TodoListItems", name: "IX_ListId", newName: "IX_ParentListId");
            RenameColumn(table: "dbo.TodoListItems", name: "ListId", newName: "ParentListId");
        }
    }
}
