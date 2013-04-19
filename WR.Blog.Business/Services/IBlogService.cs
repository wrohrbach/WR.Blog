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
        /// <param name="id">The id of the version to delete.</param>
        void DeleteVersion(int id);
        #endregion
    }
}
