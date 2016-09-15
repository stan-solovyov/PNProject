namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SocialNetworkUserId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "SocialNetworkUserId", c => c.String());
            DropColumn("dbo.Users", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "SocialNetworUserId", c => c.String());
            DropColumn("dbo.Users", "SocialNetworkUserId");
        }
    }
}
