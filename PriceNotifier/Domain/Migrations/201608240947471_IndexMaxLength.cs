namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IndexMaxLength : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Products", "IX_ExtIdAndUserId");
            AlterColumn("dbo.Products", "ExternalProductId", c => c.String(maxLength: 450));
            CreateIndex("dbo.Products", new[] { "UserId", "ExternalProductId" }, name: "IX_ExtIdAndUserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Products", "IX_ExtIdAndUserId");
            AlterColumn("dbo.Products", "ExternalProductId", c => c.String());
            CreateIndex("dbo.Products", new[] { "UserId", "ExternalProductId" }, name: "IX_ExtIdAndUserId");
        }
    }
}
