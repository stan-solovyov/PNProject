namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ValidationAttributes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Products", "ExternalProductId", c => c.String(nullable: false, maxLength: 450));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ExternalProductId", c => c.String(maxLength: 450));
            AlterColumn("dbo.Products", "Name", c => c.String());
        }
    }
}
