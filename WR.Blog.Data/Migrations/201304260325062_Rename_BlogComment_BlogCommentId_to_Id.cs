namespace WR.Blog.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rename_BlogComment_BlogCommentId_to_Id : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.BlogComments", "BlogCommentId", "Id");
            DropForeignKey("dbo.BlogComments", "ReplyToComment_BlogCommentId", "dbo.BlogComments");
            RenameColumn("dbo.BlogComments", "ReplyToComment_BlogCommentId", "ReplyToComment_Id");
            AddForeignKey("dbo.BlogComments", "ReplyToComment_Id", "dbo.BlogComments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlogComments", "ReplyToComment_Id", "dbo.BlogComments");
            RenameColumn("dbo.BlogComments", "ReplyToComment_Id", "ReplyToComment_BlogCommentId");
            AddForeignKey("dbo.BlogComments", "ReplyToComment_BlogCommentId", "dbo.BlogComments", "BlogCommentId");
            RenameColumn("dbo.BlogComments", "Id", "BlogCommentId");
        }
    }
}
