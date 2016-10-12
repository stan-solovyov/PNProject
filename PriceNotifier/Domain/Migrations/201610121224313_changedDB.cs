namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedDB : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PriceHistories", "Product_ProductId", "dbo.Products");
            DropIndex("dbo.PriceHistories", new[] { "Product_ProductId" });
            DropColumn("dbo.PriceHistories", "Product_ProductId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PriceHistories", "Product_ProductId", c => c.Int());
            CreateIndex("dbo.PriceHistories", "Product_ProductId");
            AddForeignKey("dbo.PriceHistories", "Product_ProductId", "dbo.Products", "ProductId");
        }
    }
}
