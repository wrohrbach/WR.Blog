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
        /// Gets the blog page by id.
        /// </summary>
        /// <param name="id">The id of the blog page to get.</param>
        /// <returns>The specified blog page.</returns>
        BlogPage GetBlogPage(int id);

        /// <summary>
        /// Gets all blog pages using supplied skip, take, and published values.
        /// </summary>
        /// <param name="skip">Number of groups of blog pages to skip (skip * take).</param>
        /// <param name="take">Number of blog pages in a group.</param>
        /// <param name="published">If set to true, only return published blog pages.</param>
        /// <param name="content">If set to true, also return content pages.</param>
        /// <returns>
        /// Returns a collection of blog pages.
        /// </returns>
        IEnumerable<BlogPage> GetBlogPages(int? skip, int? take, bool published = false, bool content = false);

        /// <summary>
        /// Gets the blog page by URL segment.
        /// </summary>
        /// <param name="urlSegment">The url segment to search by.</param>
        /// <param name="isContentPage">If set to true, only return a content page.</param>
        /// <returns>Returns the first blog page encountered by url segment.</returns>
        BlogPage GetBlogPageByUrlSegment(string urlSegment, bool isContentPage = false);

        /// <summary>
        /// Gets the blog pages by permalink information.
        /// </summary>
        /// <param name="year">The year to search. If year is null, null is returned.</param>
        /// <param name="month">The month to search. Pass null to search by year.</param>
        /// <param name="day">The day to search. Pass null to search by month.</param>
        /// <param name="urlSegment">The url segment derived from the blog post title.</param>
        /// <param name="isPublished">If true, only return blog pages marked as published.</param>
        /// <returns>Returns a collection of blog pages published within the specified date range.</returns>
        BlogPage GetBlogPageByPermalink(int? year, int? month, int? day, string urlSegment, bool isPublished = true);

        /// <summary>
        /// Gets the blog pages by date.
        /// </summary>
        /// <param name="year">The year to search. If year is null, null is returned.</param>
        /// <param name="month">The month to search. Pass null to search by year.</param>
        /// <param name="day">The day to search. Pass null to search by month.</param>
        /// <param name="isPublished">If true, only return blog pages where the IsPublished flag is set.</param>
        /// <returns>
        /// Returns a collection of blog pages published within the specified date range.
        /// </returns>
        IEnumerable<BlogPage> GetBlogPagesByDate(int? year, int? month, int? day, bool isPublished = true);

        /// <summary>
        /// Adds the blog page.
        /// </summary>
        /// <param name="blogPage">The blog page.</param>
        void AddBlogPage(BlogPage blogPage);

        /// <summary>
        /// Updates the blog page.
        /// </summary>
        /// <param name="blogPage">The blog page.</param>
        void UpdateBlogPage(BlogPage blogPage);

        /// <summary>
        /// Deletes the blog page.
        /// </summary>
        /// <param name="id">The id of the blog page to delete.</param>
        void DeleteBlogPage(int id);
        #endregion
    }
}
