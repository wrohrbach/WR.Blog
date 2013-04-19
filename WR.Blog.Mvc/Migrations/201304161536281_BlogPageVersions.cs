namespace WR.Blog.Mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BlogPageVersions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlogPageVersions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        UrlSegment = c.String(maxLength: 100),
                        Text = c.String(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        PublishedDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(nullable: false),
                        AllowComments = c.Boolean(nullable: false),
                        IsContentPage = c.Boolean(nullable: false),
                        VersionOf_Id = c.Int(),
                        Author_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BlogPages", t => t.VersionOf_Id)
                .ForeignKey("dbo.UserProfile", t => t.Author_UserId)
                .Index(t => t.VersionOf_Id)
                .Index(t => t.Author_UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.BlogPageVersions", new[] { "Author_UserId" });
            DropIndex("dbo.BlogPageVersions", new[] { "VersionOf_Id" });
            DropForeignKey("dbo.BlogPageVersions", "Author_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.BlogPageVersions", "VersionOf_Id", "dbo.BlogPages");
            DropTable("dbo.BlogPageVersions");
        }
    }
}
