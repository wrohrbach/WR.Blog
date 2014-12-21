using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WR.Blog.Data.Models;

namespace WR.Blog.Business.Services
{
    public interface IBlogService : IBaseService, IDisposable
    {
        #region Blog Methods
        /// <summary>
        /// Gets the blog post by id.
        /// </summary>
        /// <param name="id">The id of the blog post to get.</param>
        /// <returns>The specified blog post.</returns>
        BlogPostDto GetBlogPost(int id);

        /// <summary>
        /// Gets all blog posts using supplied skip, take, published, content, and orderByPublishedDesc values.
        /// </summary>
        /// <param name="skip">Number of groups of blog posts to skip (skip * take).</param>
        /// <param name="take">Number of blog posts in a group.</param>
        /// <param name="published">If set to true, only return published blog posts.</param>
        /// <param name="content">If set to true, also return content pages.</param>
        /// <param name="orderByPublishedDescending">If set to true, order by published date descending.</param>
        /// <returns>
        /// Returns a collection of blog posts.
        /// </returns>
        IEnumerable<BlogPostDto> GetBlogPosts(int? skip, int? take, bool published = false, bool content = false, bool orderByPublishedDescending = true);

        /// <summary>
        /// Gets the blog post by URL segment.
        /// </summary>
        /// <param name="urlSegment">The url segment to search by.</param>
        /// <param name="isContentPage">If set to true, only return a content .</param>
        /// <returns>Returns the first blog post encountered by url segment.</returns>
        BlogPostDto GetBlogPostByUrlSegment(string urlSegment, bool isContentPage = false);

        /// <summary>
        /// Gets the blog posts by permalink information (excludes content pages).
        /// </summary>
        /// <param name="year">The year to search. If year is null, null is returned.</param>
        /// <param name="month">The month to search. Pass null to search by year.</param>
        /// <param name="day">The day to search. Pass null to search by month.</param>
        /// <param name="urlSegment">The url segment derived from the blog post title.</param>
        /// <param name="isPublished">If true, only return blog posts marked as published.</param>
        /// <returns>Returns a collection of blog posts published within the specified date range.</returns>
        BlogPostDto GetBlogPostByPermalink(int? year, int? month, int? day, string urlSegment, bool isPublished = true);

        /// <summary>
        /// Gets the blog posts by date (excludes content pages).
        /// </summary>
        /// <param name="year">The year to search. If year is null, null is returned.</param>
        /// <param name="month">The month to search. Pass null to search by year.</param>
        /// <param name="day">The day to search. Pass null to search by month.</param>
        /// <param name="isPublished">If true, only return blog posts where the IsPublished flag is set.</param>
        /// <returns>
        /// Returns a collection of blog posts published within the specified date range.
        /// </returns>
        IEnumerable<BlogPostDto> GetBlogPostsByDate(int? year, int? month, int? day, bool isPublished = true);

        /// <summary>
        /// Adds the blog post.
        /// </summary>
        /// <param name="blogPost">The blog post to add.</param>
        void AddBlogPost(BlogPostDto blogPost);

        /// <summary>
        /// Updates the blog post.
        /// </summary>
        /// <param name="blogPost">The blog post to update.</param>
        void UpdateBlogPost(BlogPostDto blogPost);

        /// <summary>
        /// Deletes the blog post.
        /// </summary>
        /// <param name="id">The id of the blog post to delete.</param>
        void DeleteBlogPost(int id);
        #endregion

        #region Blog Version Methods
        /// <summary>
        /// Gets a blog post version.
        /// </summary>
        /// <param name="id">The version id.</param>
        /// <returns>The specified version by id.</returns>
        BlogVersionDto GetVersion(int id);

        /// <summary>
        /// Gets the versions for the specified blog post.
        /// </summary>
        /// <param name="blogPost">The blog post to retrieve versions for.</param>
        /// <returns>A collection of versions for the given blog post.</returns>
        IEnumerable<BlogVersionDto> GetVersionsByBlogPost(BlogPostDto blogPost);

        /// <summary>
        /// Gets the versions for the specified blog post.
        /// </summary>
        /// <param name="blogPostId">The id of the blog post to retrieve versions for.</param>
        /// <returns>
        /// A collection of versions for the given blog post.
        /// </returns>
        IEnumerable<BlogVersionDto> GetVersionsByBlogPost(int blogPostId);

        /// <summary>
        /// Saves the blog post as a version.
        /// </summary>
        /// <param name="blogPost">The blog post to save as a version.</param>
        /// <returns>The new version id.</returns>
        int SaveBlogPostAsVersion(BlogPostDto blogPost);

        /// <summary>
        /// Saves the blog post as a version.
        /// </summary>
        /// <param name="blogPostId">The id of the blog post to save as a version.</param>
        /// <returns>The new version id.</returns>
        int SaveBlogPostAsVersion(int blogPostId);

        /// <summary>
        /// Publishes the supplied version.
        /// </summary>
        /// <param name="version">The version to publish.</param>
        void PublishVersion(BlogVersionDto version);

        /// <summary>
        /// Updates and saves changes to the version.
        /// </summary>
        /// <param name="version">The modified version to update.</param>
        void UpdateVersion(BlogVersionDto version);

        /// <summary>
        /// Deletes the specified version.
        /// </summary>
        /// <param name="versionId">The id of the version to delete.</param>
        void DeleteVersion(int versionId);
        #endregion

        #region Blog Comment Methods
        /// <summary>
        /// Gets the comment by id.
        /// </summary>
        /// <param name="commentId">The comment id.</param>
        /// <returns>Returns the comment.</returns>
        BlogCommentDto GetComment(int commentId);

        /// <summary>
        /// Gets the comments by blog post.
        /// </summary>
        /// <param name="blogPost">The blog post to get comments for.</param>
        /// <param name="isApproved">If true, only return comments that have been approved by a moderator.</param>
        /// <param name="isDeleted">If false, only return comments that are not marked as deleted.</param>
        /// <returns>Returns a collection of blog comments associated with a blog post.</returns>
        IEnumerable<BlogCommentDto> GetCommentsByBlogPost(BlogPostDto blogPost, bool isApproved = true, bool isDeleted = false);

        /// <summary>
        /// Gets the comments by blog post.
        /// </summary>
        /// <param name="blogPostId">The blog post id to get comments for.</param>
        /// <param name="isApproved">If true, only return comments that have been approved by a moderator.</param>
        /// <param name="isDeleted">If false, only return comments that are not marked as deleted.</param>
        /// <returns>
        /// Returns a collection of blog comments associated with a blog post.
        /// </returns>
        IEnumerable<BlogCommentDto> GetCommentsByBlogPost(int blogPostId, bool isApproved = true, bool isDeleted = false);

        /// <summary>
        /// Gets the number of comments for a blog post.
        /// </summary>
        /// <param name="blogPostId">The id of the blog post to count comments.</param>
        /// <param name="isApproved">If true, only count comments that have been approved by a moderator.</param>
        /// <returns>
        /// The number of comments for this blog post.
        /// </returns>
        int GetCommentCount(int blogPostId, bool isApproved = true);

        /// <summary>
        /// Gets all comments that have not been approved yet.
        /// </summary>
        /// <returns>Returns a collection of blog comments.</returns>
        IEnumerable<BlogCommentDto> GetUnapprovedComments();

        /// <summary>
        /// Gets comments that have not been approved yet filtered by blog post id.
        /// </summary>
        /// <param name="blogPostId">Filter the unapproved comments by the blog post id.</param>
        /// <returns>Returns a collection of blog comments filtered by blog post id.</returns>
        IEnumerable<BlogCommentDto> GetUnapprovedComments(int blogPostId);

        /// <summary>
        /// Gets the number of unapproved comments.
        /// </summary>
        /// <returns>Returns how many comments have not been approved.</returns>
        int GetUnapprovedCommentCount();

        /// <summary>
        /// Gets the number of unapproved comments.
        /// </summary>
        /// <param name="blogPostId">The id of the blog post to count comments.</param>
        /// <returns>
        /// Returns how many comments have not been approved.
        /// </returns>
        int GetUnapprovedCommentCount(int blogPostId);

        /// <summary>
        /// Approves the comment for publishing.
        /// </summary>
        /// <param name="commentId">The id of the comment to approve.</param>
        /// <param name="isApproved">Set to false to unapprove a comment. Leave as true to approve.</param>
        void ApproveComment(int commentId, bool isApproved = true);

        /// <summary>
        /// Adds the comment.
        /// </summary>
        /// <param name="comment">The comment to add.</param>
        /// <returns>The id of the newly added comment.</returns>
        int AddComment(BlogCommentDto comment);

        /// <summary>
        /// Updates the comment.
        /// </summary>
        /// <param name="comment">The comment to update.</param>
        void UpdateComment(BlogCommentDto comment);

        /// <summary>
        /// Marks the comment as deleted.
        /// </summary>
        /// <param name="commentId">The id of the comment to mark as deleted.</param>
        void DeleteComment(int commentId);
        #endregion
    }
}
