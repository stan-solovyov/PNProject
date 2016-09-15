namespace Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ManyToMany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "UserId", "dbo.Users");
            DropIndex("dbo.Products", "IX_ExtIdAndUserId");
            CreateTable(
                "dbo.UserProducts",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ProductId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ProductId);
            
            DropColumn("dbo.Products", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "UserId", c => c.Int(nullable: false));
            DropForeignKey("dbo.UserProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.UserProducts", "UserId", "dbo.Users");
            DropIndex("dbo.UserProducts", new[] { "ProductId" });
            DropIndex("dbo.UserProducts", new[] { "UserId" });
            DropTable("dbo.UserProducts");
            CreateIndex("dbo.Products", new[] { "UserId", "ExternalProductId" }, name: "IX_ExtIdAndUserId");
            AddForeignKey("dbo.Products", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
