namespace Todo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeignKeyDefinition : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TodoListItems", "List_Id", "dbo.TodoLists");
            DropForeignKey("dbo.TodoLists", "Owner_Id", "dbo.Users");
            DropIndex("dbo.TodoListItems", new[] { "List_Id" });
            DropIndex("dbo.TodoLists", new[] { "Owner_Id" });
            RenameColumn(table: "dbo.TodoListItems", name: "List_Id", newName: "ParentListId");
            RenameColumn(table: "dbo.TodoLists", name: "Owner_Id", newName: "OwnerId");
            AlterColumn("dbo.TodoListItems", "ParentListId", c => c.Guid(nullable: false));
            AlterColumn("dbo.TodoLists", "OwnerId", c => c.Guid(nullable: false));
            CreateIndex("dbo.TodoListItems", "ParentListId");
            CreateIndex("dbo.TodoLists", "OwnerId");
            AddForeignKey("dbo.TodoListItems", "ParentListId", "dbo.TodoLists", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TodoLists", "OwnerId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TodoLists", "OwnerId", "dbo.Users");
            DropForeignKey("dbo.TodoListItems", "ParentListId", "dbo.TodoLists");
            DropIndex("dbo.TodoLists", new[] { "OwnerId" });
            DropIndex("dbo.TodoListItems", new[] { "ParentListId" });
            AlterColumn("dbo.TodoLists", "OwnerId", c => c.Guid());
            AlterColumn("dbo.TodoListItems", "ParentListId", c => c.Guid());
            RenameColumn(table: "dbo.TodoLists", name: "OwnerId", newName: "Owner_Id");
            RenameColumn(table: "dbo.TodoListItems", name: "ParentListId", newName: "List_Id");
            CreateIndex("dbo.TodoLists", "Owner_Id");
            CreateIndex("dbo.TodoListItems", "List_Id");
            AddForeignKey("dbo.TodoLists", "Owner_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.TodoListItems", "List_Id", "dbo.TodoLists", "Id");
        }
    }
}
