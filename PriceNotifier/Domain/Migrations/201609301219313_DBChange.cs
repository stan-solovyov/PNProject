namespace Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class DBChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PriceHistories", "Product_ProductId", "dbo.Products");
            DropIndex("dbo.PriceHistories", new[] { "Product_ProductId" });
            RenameColumn(table: "dbo.PriceHistories", name: "Product_ProductId", newName: "ProductId");
            AlterColumn("dbo.PriceHistories", "ProductId", c => c.Int(nullable: false));
            CreateIndex("dbo.PriceHistories", "ProductId");
            AddForeignKey("dbo.PriceHistories", "ProductId", "dbo.Products", "ProductId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PriceHistories", "ProductId", "dbo.Products");
            DropIndex("dbo.PriceHistories", new[] { "ProductId" });
            AlterColumn("dbo.PriceHistories", "ProductId", c => c.Int());
            RenameColumn(table: "dbo.PriceHistories", name: "ProductId", newName: "Product_ProductId");
            CreateIndex("dbo.PriceHistories", "Product_ProductId");
            AddForeignKey("dbo.PriceHistories", "Product_ProductId", "dbo.Products", "ProductId");
        }
    }
}
