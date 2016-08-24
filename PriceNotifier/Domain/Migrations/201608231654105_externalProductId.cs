namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class externalProductId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ExternalProductId", c => c.String());
            DropColumn("dbo.Products", "ProductId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ProductId", c => c.String());
            DropColumn("dbo.Products", "ExternalProductId");
        }
    }
}
