namespace Todo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TodoListItems",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ItemName = c.String(),
                        Description = c.String(),
                        AddedOn = c.DateTime(nullable: false),
                        IsComplete = c.Boolean(nullable: false),
                        CompletedOn = c.DateTime(),
                        List_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TodoLists", t => t.List_Id)
                .Index(t => t.List_Id);
            
            CreateTable(
                "dbo.TodoLists",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        CreatedOn = c.DateTime(nullable: false),
                        Owner_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Owner_Id)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 100),
                        FirstName = c.String(nullable: false, maxLength: 200),
                        LastName = c.String(nullable: false, maxLength: 200),
                        BirthDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TodoLists", "Owner_Id", "dbo.Users");
            DropForeignKey("dbo.TodoListItems", "List_Id", "dbo.TodoLists");
            DropIndex("dbo.TodoLists", new[] { "Owner_Id" });
            DropIndex("dbo.TodoListItems", new[] { "List_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.TodoLists");
            DropTable("dbo.TodoListItems");
        }
    }
}
