namespace WR.Blog.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rename_SiteSettings_to_BlogSettings : DbMigration
    {
        public override void Up()
        {
            RenameTable("SiteSettings", "BlogSettings");            
        }
        
        public override void Down()
        {
            RenameTable("BlogSettings", "SiteSettings");  
        }
    }
}
