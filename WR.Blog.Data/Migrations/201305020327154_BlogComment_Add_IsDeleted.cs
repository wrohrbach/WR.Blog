namespace WR.Blog.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BlogComment_Add_IsDeleted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogComments", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BlogComments", "IsDeleted");
        }
    }
}
