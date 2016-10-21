namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Article : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        ArticleId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Summary = c.String(nullable: false),
                        Body = c.String(nullable: false),
                        ImageUrl = c.String(),
                        IsPublished = c.Boolean(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ArticleId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Articles", "ProductId", "dbo.Products");
            DropIndex("dbo.Articles", new[] { "ProductId" });
            DropTable("dbo.Articles");
        }
    }
}
