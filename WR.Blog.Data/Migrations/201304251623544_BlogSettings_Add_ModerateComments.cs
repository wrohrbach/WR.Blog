namespace WR.Blog.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BlogSettings_Add_ModerateComments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogSettings", "ModerateComments", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BlogSettings", "ModerateComments");
        }
    }
}
