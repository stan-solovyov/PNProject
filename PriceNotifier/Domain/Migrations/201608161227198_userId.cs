namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "User_Id", "dbo.Users");
            DropIndex("dbo.Products", new[] { "User_Id" });
            RenameColumn(table: "dbo.Products", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.Products", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "UserId");
            AddForeignKey("dbo.Products", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "UserId", "dbo.Users");
            DropIndex("dbo.Products", new[] { "UserId" });
            AlterColumn("dbo.Products", "UserId", c => c.Int());
            RenameColumn(table: "dbo.Products", name: "UserId", newName: "User_Id");
            CreateIndex("dbo.Products", "User_Id");
            AddForeignKey("dbo.Products", "User_Id", "dbo.Users", "Id");
        }
    }
}
