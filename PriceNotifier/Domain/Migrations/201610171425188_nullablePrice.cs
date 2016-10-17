namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullablePrice : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProvidersProductInfoes", "MinPrice", c => c.Double());
            AlterColumn("dbo.ProvidersProductInfoes", "MaxPrice", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProvidersProductInfoes", "MaxPrice", c => c.Double(nullable: false));
            AlterColumn("dbo.ProvidersProductInfoes", "MinPrice", c => c.Double(nullable: false));
        }
    }
}
