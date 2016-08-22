namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProductsList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ImageUrl", c => c.String());
            AddColumn("dbo.Products", "User_Id", c => c.Int());
            CreateIndex("dbo.Products", "User_Id");
            AddForeignKey("dbo.Products", "User_Id", "dbo.Users", "Id");
            DropColumn("dbo.Products", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Image", c => c.Binary());
            DropForeignKey("dbo.Products", "User_Id", "dbo.Users");
            DropIndex("dbo.Products", new[] { "User_Id" });
            DropColumn("dbo.Products", "User_Id");
            DropColumn("dbo.Products", "ImageUrl");
        }
    }
}
