using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AutoMapper;
using WR.Blog.Data.Models;
using WR.Blog.Data.Repositories;

namespace WR.Blog.Business.Services
{
    public class BlogService : BaseService, IBlogService
    {
        public BlogService(IBlogRepository br) : base(br)
        { }

        #region Blog Methods
        /// <summary>
        /// Gets the blog post by id.
        /// </summary>
        /// <param name="blogPostId">The id of the blog post to get.</param>
        /// <returns>
        /// The specified blog post.
        /// </returns>
        public BlogPostDto GetBlogPost(int blogPostId)
        {
            return br.GetBlogPostById(blogPostId);            
        }

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
        public IEnumerable<BlogPostDto> GetBlogPosts(int? skip, int? take, bool published = false, bool content = false, bool orderByPublishedDescending = true)
        {
            var blogPosts = br.GetBlogPosts();

            if (!content)
            {
                blogPosts = blogPosts.Where(b => !b.IsContentPage);
            }

            if (published)
            {
                blogPosts = blogPosts.Where(b => b.IsPublished && b.PublishedDate <= DateTime.Now);
            }

            if (orderByPublishedDescending)
            {
                blogPosts = blogPosts.OrderByDescending(b => b.PublishedDate);
            }

            if (skip.HasValue && take.HasValue)
            {
                skip = (skip.Value - 1) * take.Value;
                blogPosts = blogPosts.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                blogPosts = blogPosts.Take(take.Value);
            }

            return blogPosts.ToList(); 
        }

        /// <summary>
        /// Gets the blog post by URL segment.
        /// </summary>
        /// <param name="urlSegment">The url segment to search by.</param>
        /// <param name="isContentPage">If set to true, only return a content page.</param>
        /// <returns>
        /// Returns the first blog post encountered by url segment.
        /// </returns>
        public BlogPostDto GetBlogPostByUrlSegment(string urlSegment, bool isContentPage = false)
        {
            var blogPosts = br.GetBlogPosts();

            if (isContentPage)
            {
                blogPosts = blogPosts.Where(b => b.IsContentPage == true);
            }

            return blogPosts.Where(b => b.UrlSegment == urlSegment).FirstOrDefault();             
        }

        /// <summary>
        /// Gets the blog posts by permalink information (excludes content pages).
        /// </summary>
        /// <param name="year">The year to search. If year is null, null is returned.</param>
        /// <param name="month">The month to search. Pass null to search by year.</param>
        /// <param name="day">The day to search. Pass null to search by month.</param>
        /// <param name="urlSegment">The url segment derived from the blog post title.</param>
        /// <param name="isPublished">If true, only return blog posts marked as published.</param>
        /// <returns>
        /// Returns a collection of blog posts published within the specified date range.
        /// </returns>
        public BlogPostDto GetBlogPostByPermalink(int? year, int? month, int? day, string urlSegment, bool isPublished = true)
        {
            var blogPosts = GetBlogPostsByDate(year, month, day, isPublished);

            if (blogPosts == null)
            {
                return null;
            }

            if (isPublished)
            {
                blogPosts = blogPosts.Where(b => b.IsPublished);
            }

            return blogPosts.Where(b => b.UrlSegment == urlSegment).FirstOrDefault();
        }
        
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
        public IEnumerable<BlogPostDto> GetBlogPostsByDate(int? year, int? month, int? day, bool isPublished = true)
        {
            int y = year ?? DateTime.MinValue.Year;
            int m = month ?? 0;
            int d = day ?? 0;

            if (y == DateTime.MinValue.Year || (y == DateTime.MinValue.Year && m == 0 && d == 0))
            {
                return null;
            }

            DateTime dateFrom;
            DateTime dateTo;

            try
            {
                if (m == 0)
                {
                    dateFrom = new DateTime(y, 1, 1);
                    dateTo = new DateTime(y, 12, 31, 23, 59, 59);
                }
                else if (d == 0)
                {
                    dateFrom = new DateTime(y, m, 1);
                    dateTo = dateFrom.AddMonths(1).AddSeconds(-1);
                }
                else
                {
                    dateFrom = new DateTime(y, m, d);
                    dateTo = dateFrom.AddDays(1).AddSeconds(-1);
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }

            var blogPosts = br.GetBlogPosts().Where(b => b.PublishedDate >= dateFrom && b.PublishedDate <= dateTo && !b.IsContentPage);
        
            if (isPublished)
            {
                blogPosts = blogPosts.Where(b => b.IsPublished);
            }

            return blogPosts.ToList();
        }

        /// <summary>
        /// Adds the blog post.
        /// </summary>
        /// <param name="blogPost">The blog post to add.</param>
        public void AddBlogPost(BlogPostDto blogPost)
        {
            br.AddBlogPost(blogPost);            
        }

        /// <summary>
        /// Updates the blog post.
        /// </summary>
        /// <param name="blogPost">The blog post to update.</param>
        public void UpdateBlogPost(BlogPostDto blogPost)
        {
            br.UpdateBlogPost(blogPost);
        }

        /// <summary>
        /// Deletes the blog post.
        /// </summary>
        /// <param name="versionId">The id of the blog post to delete.</param>
        public void DeleteBlogPost(int versionId)
        {
            br.DeleteAllBlogPostVersions(versionId);
            br.DeleteBlogPost(versionId);
        }
        #endregion

        #region Blog Version Methods
        /// <summary>
        /// Gets a blog post version.
        /// </summary>
        /// <param name="versionId">The version id.</param>
        /// <returns>
        /// The specified version by id.
        /// </returns>
        public BlogVersionDto GetVersion(int versionId)
        {
            return br.GetBlogPostVersionById(versionId);
        }

        /// <summary>
        /// Saves the blog post as a version.
        /// </summary>
        /// <param name="blogPost">The blog post to save as a version.</param>
        /// <returns>
        /// The new version id.
        /// </returns>
        public int SaveBlogPostAsVersion(BlogPostDto blogPost)
        {
            BlogVersionDto version = Mapper.Map<BlogVersionDto>(blogPost);
            version.VersionOf = blogPost;

            return br.AddBlogPostVersion(version);
        }

        /// <summary>
        /// Saves the blog post as a version.
        /// </summary>
        /// <param name="blogPostId">The id of the blog post to save as a version.</param>
        /// <returns>
        /// The new version id.
        /// </returns>
        public int SaveBlogPostAsVersion(int blogPostId)
        {
            BlogPostDto blogPost = GetBlogPost(blogPostId);

            return SaveBlogPostAsVersion(blogPost);
        }

        /// <summary>
        /// Publishes the supplied version.
        /// </summary>
        /// <param name="version">The version to publish.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void PublishVersion(BlogVersionDto version)
        {
            SaveBlogPostAsVersion(version.VersionOf);

            Mapper.Map<BlogVersionDto, BlogPostDto>(version, version.VersionOf);

            br.UpdateBlogPostVersion(version);
        }

        /// <summary>
        /// Gets the versions for the specified blog post.
        /// </summary>
        /// <param name="blogPostId">The id of the blog post to retrieve versions for.</param>
        /// <returns>
        /// A collection of versions for the given blog post.
        /// </returns>
        public IEnumerable<BlogVersionDto> GetVersionsByBlogPost(int blogPostId)
        {
            return br.GetBlogPostVersions()
                .Where(v => v.VersionOf.Id == blogPostId)
                .OrderByDescending(v => v.LastModifiedDate)
                .ToList();
        }

        /// <summary>
        /// Gets the versions for the specified blog post.
        /// </summary>
        /// <param name="blogPost">The blog post to retrieve versions for.</param>
        /// <returns>
        /// A collection of versions for the given blog post.
        /// </returns>
        public IEnumerable<BlogVersionDto> GetVersionsByBlogPost(BlogPostDto blogPost)
        {
            return GetVersionsByBlogPost(blogPost.Id);
        }

        /// <summary>
        /// Updates and saves changes to the version.
        /// </summary>
        /// <param name="version">The modified version to update.</param>
        public void UpdateVersion(BlogVersionDto version)
        {
            br.UpdateBlogPostVersion(version);
        }

        /// <summary>
        /// Deletes the specified version.
        /// </summary>
        /// <param name="versionId">The id of the version to delete.</param>
        public void DeleteVersion(int versionId)
        {
            br.DeleteBlogPostVersion(versionId);
        }
        #endregion

        #region Blog Comment Methods
        /// <summary>
        /// Gets the comment by id.
        /// </summary>
        /// <param name="commentId">The comment id.</param>
        /// <returns>
        /// Returns the comment.
        /// </returns>
        public BlogCommentDto GetComment(int commentId)
        {
            return br.GetBlogCommentById(commentId);
        }

        /// <summary>
        /// Gets the comments by blog post.
        /// </summary>
        /// <param name="blogPost">The blog post to get comments for.</param>
        /// <param name="isApproved">If true, only return comments that have been approved by a moderator.</param>
        /// <param name="isDeleted">If false, only return comments that are not marked as deleted.</param>
        /// <returns>
        /// Returns a collection of blog comments associated with a blog post.
        /// </returns>
        public IEnumerable<BlogCommentDto> GetCommentsByBlogPost(BlogPostDto blogPost, bool isApproved = true, bool isDeleted = false)
        {
            return GetCommentsByBlogPost(blogPost.Id, isApproved, isDeleted);
        }

        /// <summary>
        /// Gets the comments by blog post.
        /// </summary>
        /// <param name="blogPostId">The blog post id to get comments for.</param>
        /// <param name="isApproved">If true, only return comments that have been approved by a moderator.</param>
        /// <param name="isDeleted">If false, only return comments that are not marked as deleted.</param>
        /// <returns>
        /// Returns a collection of blog comments associated with a blog post.
        /// </returns>
        public IEnumerable<BlogCommentDto> GetCommentsByBlogPost(int blogPostId, bool isApproved = true, bool isDeleted = false)
        {
            // TODO: Write tests for GetCommentsByBlogPost() where isDeleted == false
            var comments = br.GetBlogComments().Where(c => c.BlogPost.Id == blogPostId);

            if (isApproved)
            {
                comments = comments.Where(c => c.IsApproved == isApproved);
            }

            if (!isDeleted)
            {
                comments = comments.Where(c => c.IsDeleted == isDeleted);
            }

            return comments.OrderBy(c => c.CommentDate)
                .ToList();
        }

        /// <summary>
        /// Gets the number of comments for a blog post.
        /// </summary>
        /// <param name="blogPostId">The id of the blog post to count comments.</param>
        /// <param name="isApproved">If true, only count comments that have been approved by a moderator.</param>
        /// <returns>
        /// The number of comments for this blog post.
        /// </returns>
        public int GetCommentCount(int blogPostId, bool isApproved = true)
        {
            var comments = br.GetBlogComments().Where(c => c.BlogPost.Id == blogPostId);

            if (isApproved)
            {
                comments = comments.Where(c => c.IsApproved == isApproved);
            }

            return comments.Count();
        }

        /// <summary>
        /// Gets all comments that have not been approved yet.
        /// </summary>
        /// <returns>
        /// Returns a collection of blog comments.
        /// </returns>
        public IEnumerable<BlogCommentDto> GetUnapprovedComments()
        {
            return br.GetBlogComments()
                .Where(c => c.IsApproved == false)
                .OrderByDescending(c => c.CommentDate)
                .ToList();
        }

        /// <summary>
        /// Gets comments that have not been approved yet filtered by blog post id.
        /// </summary>
        /// <param name="blogPostId">Filter the unapproved comments by the blog post id.</param>
        /// <returns>
        /// Returns a collection of blog comments filtered by blog post id.
        /// </returns>
        public IEnumerable<BlogCommentDto> GetUnapprovedComments(int blogPostId)
        {
            return br.GetBlogComments()
                .Where(c => c.BlogPost.Id == blogPostId && c.IsApproved == false)
                .OrderByDescending(c => c.CommentDate)
                .ToList();
        }

        /// <summary>
        /// Approves the comment for publishing.
        /// </summary>
        /// <param name="commentId">The id of the comment to approve.</param>
        /// <param name="isApproved">Set to false to unapprove a comment. Leave as true to approve.</param>
        public void ApproveComment(int commentId, bool isApproved = true)
        {
            var comment = br.GetBlogCommentById(commentId);

            if (comment.IsApproved != isApproved)
            {
                comment.IsApproved = isApproved;
                br.UpdateBlogComment(comment);
            }
        }

        /// <summary>
        /// Adds the comment.
        /// </summary>
        /// <param name="comment">The comment to add.</param>
        /// <returns>
        /// The id of the newly added comment.
        /// </returns>
        public int AddComment(BlogCommentDto comment)
        {
            return br.AddBlogComment(comment);
        }

        /// <summary>
        /// Updates the comment.
        /// </summary>
        /// <param name="comment">The comment to update.</param>
        public void UpdateComment(BlogCommentDto comment)
        {
            br.UpdateBlogComment(comment);
        }

        /// <summary>
        /// Deletes the comment.
        /// </summary>
        /// <param name="commentId">The id of the comment to delete.</param>
        public void DeleteComment(int commentId)
        {
            // TODO: Re-write DeleteComment() tests
            var comment = br.GetBlogCommentById(commentId);

            if (comment != null && !comment.IsDeleted)
            {
                comment.IsDeleted = true;
                UpdateComment(comment);
            }
        } 
        #endregion
    }
}
