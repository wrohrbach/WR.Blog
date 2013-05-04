namespace WR.Blog.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        DisplayName = c.String(),
                        EmailAddress = c.String(),
                        WebSite = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.SiteSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SiteTitle = c.String(maxLength: 100),
                        Tagline = c.String(maxLength: 500),
                        AltTagline = c.String(maxLength: 500),
                        MenuLinks = c.String(maxLength: 1000),
                        PostsPerPage = c.Int(nullable: false),
                        AllowComments = c.Boolean(nullable: false),
                        GoogleAccount = c.String(maxLength: 20),
                        LastModifiedDate = c.DateTime(nullable: false),
                        LastModifiedBy_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.LastModifiedBy_UserId)
                .Index(t => t.LastModifiedBy_UserId);
            
            CreateTable(
                "dbo.BlogPosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        UrlSegment = c.String(maxLength: 100),
                        Text = c.String(nullable: false),
                        LastModifiedDate = c.DateTime(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        PublishedDate = c.DateTime(nullable: false),
                        AllowComments = c.Boolean(nullable: false),
                        IsContentPage = c.Boolean(nullable: false),
                        Author_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.Author_UserId)
                .Index(t => t.Author_UserId);
            
            CreateTable(
                "dbo.BlogVersions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        UrlSegment = c.String(maxLength: 100),
                        Text = c.String(nullable: false),
                        LastModifiedDate = c.DateTime(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        PublishedDate = c.DateTime(nullable: false),
                        AllowComments = c.Boolean(nullable: false),
                        IsContentPage = c.Boolean(nullable: false),
                        VersionOf_Id = c.Int(),
                        Author_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BlogPosts", t => t.VersionOf_Id)
                .ForeignKey("dbo.UserProfile", t => t.Author_UserId)
                .Index(t => t.VersionOf_Id)
                .Index(t => t.Author_UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.BlogVersions", new[] { "Author_UserId" });
            DropIndex("dbo.BlogVersions", new[] { "VersionOf_Id" });
            DropIndex("dbo.BlogPosts", new[] { "Author_UserId" });
            DropIndex("dbo.SiteSettings", new[] { "LastModifiedBy_UserId" });
            DropForeignKey("dbo.BlogVersions", "Author_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.BlogVersions", "VersionOf_Id", "dbo.BlogPosts");
            DropForeignKey("dbo.BlogPosts", "Author_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.SiteSettings", "LastModifiedBy_UserId", "dbo.UserProfile");
            DropTable("dbo.BlogVersions");
            DropTable("dbo.BlogPosts");
            DropTable("dbo.SiteSettings");
            DropTable("dbo.UserProfile");
        }
    }
}
