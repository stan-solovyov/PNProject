namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullablePriceHistory : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PriceHistories", "OldPrice", c => c.Double());
            AlterColumn("dbo.PriceHistories", "NewPrice", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PriceHistories", "NewPrice", c => c.Double(nullable: false));
            AlterColumn("dbo.PriceHistories", "OldPrice", c => c.Double(nullable: false));
        }
    }
}
