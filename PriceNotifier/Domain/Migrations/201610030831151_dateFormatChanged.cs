namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dateFormatChanged : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PriceHistories",
                c => new
                    {
                        PriceHistoryId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        OldPrice = c.Double(nullable: false),
                        NewPrice = c.Double(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PriceHistoryId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Double(nullable: false),
                        ExternalProductId = c.String(maxLength: 450),
                        Url = c.String(),
                        ImageUrl = c.String(),
                    })
                .PrimaryKey(t => t.ProductId);
            
            CreateTable(
                "dbo.UserProducts",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Checked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ProductId })
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 25),
                        SocialNetworkUserId = c.String(),
                        SocialNetworkName = c.String(),
                        Token = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProducts", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.PriceHistories", "ProductId", "dbo.Products");
            DropIndex("dbo.UserProducts", new[] { "ProductId" });
            DropIndex("dbo.UserProducts", new[] { "UserId" });
            DropIndex("dbo.PriceHistories", new[] { "ProductId" });
            DropTable("dbo.Users");
            DropTable("dbo.UserProducts");
            DropTable("dbo.Products");
            DropTable("dbo.PriceHistories");
        }
    }
}
