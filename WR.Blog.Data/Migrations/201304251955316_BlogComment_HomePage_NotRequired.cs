namespace WR.Blog.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BlogComment_HomePage_NotRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BlogComments", "Homepage", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BlogComments", "Homepage", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
