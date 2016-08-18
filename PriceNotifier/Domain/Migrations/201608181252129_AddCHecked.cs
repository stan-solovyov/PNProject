namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCHecked : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Checked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Checked");
        }
    }
}
