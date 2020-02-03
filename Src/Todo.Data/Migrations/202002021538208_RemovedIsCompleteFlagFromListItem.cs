namespace Todo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedIsCompleteFlagFromListItem : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TodoListItems", "IsComplete");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TodoListItems", "IsComplete", c => c.Boolean(nullable: false));
        }
    }
}
