namespace Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class DateFrormatChanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PriceHistories", "Date", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PriceHistories", "Date", c => c.DateTime(nullable: false));
        }
    }
}
