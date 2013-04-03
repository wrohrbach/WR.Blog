namespace WR.Blog.Mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GoogleAccountSiteSetting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SiteSettings", "GoogleAccount", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SiteSettings", "GoogleAccount");
        }
    }
}
