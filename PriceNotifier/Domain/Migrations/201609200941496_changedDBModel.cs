namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedDBModel : DbMigration
    {
        public override void Up()
        {
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
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProducts", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserProducts", "ProductId", "dbo.Products");
            DropIndex("dbo.UserProducts", new[] { "ProductId" });
            DropIndex("dbo.UserProducts", new[] { "UserId" });
            DropTable("dbo.Users");
            DropTable("dbo.UserProducts");
            DropTable("dbo.Products");
        }
    }
}
