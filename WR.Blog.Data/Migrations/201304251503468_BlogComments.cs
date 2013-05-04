namespace WR.Blog.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BlogComments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlogComments",
                c => new
                    {
                        BlogCommentId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 100),
                        GravatarHash = c.String(),
                        Homepage = c.String(nullable: false, maxLength: 100),
                        Title = c.String(maxLength: 100),
                        Comment = c.String(nullable: false),
                        CommentDate = c.DateTime(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        BlogPost_Id = c.Int(),
                        ReplyToComment_BlogCommentId = c.Int(),
                    })
                .PrimaryKey(t => t.BlogCommentId)
                .ForeignKey("dbo.BlogPosts", t => t.BlogPost_Id)
                .ForeignKey("dbo.BlogComments", t => t.ReplyToComment_BlogCommentId)
                .Index(t => t.BlogPost_Id)
                .Index(t => t.ReplyToComment_BlogCommentId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.BlogComments", new[] { "ReplyToComment_BlogCommentId" });
            DropIndex("dbo.BlogComments", new[] { "BlogPost_Id" });
            DropForeignKey("dbo.BlogComments", "ReplyToComment_BlogCommentId", "dbo.BlogComments");
            DropForeignKey("dbo.BlogComments", "BlogPost_Id", "dbo.BlogPosts");
            DropTable("dbo.BlogComments");
        }
    }
}
