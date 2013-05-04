namespace WR.Blog.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BlogComment_Add_IpAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogComments", "IpAddress", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BlogComments", "IpAddress");
        }
    }
}
