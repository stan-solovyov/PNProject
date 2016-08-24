namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Index : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Products", new[] { "UserId" });
            CreateIndex("dbo.Products", new[] { "UserId", "ExternalProductId" }, name: "IX_ExtIdAndUserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Products", "IX_ExtIdAndUserId");
            CreateIndex("dbo.Products", "UserId");
        }
    }
}
