using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WR.Blog.Data.Models;
using WR.Blog.Data.Repositories;

namespace WR.Blog.Business.Services
{
    public class BlogService : BaseService, IBlogService
    {
        public BlogService(IBlogRepository br) : base(br)
        { }

        #region Blog Methods

        public BlogPage GetBlogPage(int id)
        {
                return br.GetBlogPageById(id);
            
        }

        /// <summary>
        /// Gets all blog pages that are currently published.
        /// </summary>
        /// <param name="published">If set to true, only return published blog pages.</param>
        /// <param name="content">If set to true, also return content pages.</param>
        /// <returns>
        /// Returns a collection of published blog pages.
        /// </returns>
        private IQueryable<BlogPage> GetBlogPages(bool published = false, bool content = false)
        {
            var blogPages = br.GetBlogPages();

            if (!content)
            {
                blogPages = blogPages.Where(b => !b.IsContentPage);
            }

            if (published)
            {
                blogPages = blogPages.Where(b => b.IsPublished && b.PublishedDate <= DateTime.Now);
            }

            return blogPages.OrderByDescending(b => b.PublishedDate);
        }

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
        public IEnumerable<BlogPage> GetBlogPages(int? skip, int? take, bool published = false, bool content = false)
        {
            var blogPages = GetBlogPages(published, content);

            if (skip.HasValue && take.HasValue)
            {
                skip = (skip.Value - 1) * take.Value;
                blogPages = blogPages.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                blogPages = blogPages.Take(take.Value);
            }

            return blogPages.ToList(); 
        }

        /// <summary>
        /// Gets the blog page by URL segment.
        /// </summary>
        /// <param name="urlSegment">The url segment to search by.</param>
        /// <param name="isContentPage">If set to true, only return a content page.</param>
        /// <returns>
        /// Returns the first blog page encountered by url segment.
        /// </returns>
        public BlogPage GetBlogPageByUrlSegment(string urlSegment, bool isContentPage = false)
        {
                var blogPages = br.GetBlogPages();

                if (isContentPage)
                {
                    blogPages = blogPages.Where(b => b.IsContentPage == true);
                }

                return blogPages.Where(b => b.UrlSegment == urlSegment).FirstOrDefault(); 
            
        }

        /// <summary>
        /// Gets the blog pages by permalink information.
        /// </summary>
        /// <param name="year">The year to search. If year is null, null is returned.</param>
        /// <param name="month">The month to search. Pass null to search by year.</param>
        /// <param name="day">The day to search. Pass null to search by month.</param>
        /// <param name="urlSegment">The url segment derived from the blog post title.</param>
        /// <param name="isPublished">If true, only return blog pages marked as published.</param>
        /// <returns>
        /// Returns a collection of blog pages published within the specified date range.
        /// </returns>
        public BlogPage GetBlogPageByPermalink(int? year, int? month, int? day, string urlSegment, bool isPublished = true)
        {
            var blogPages = GetBlogPagesByDate(year, month, day);

            if (blogPages == null)
            {
                return null;
            }

            if (isPublished)
            {
                blogPages = blogPages.Where(b => b.IsPublished);
            }

            return blogPages.Where(b => b.UrlSegment == urlSegment).FirstOrDefault();
        }
        
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
        public IEnumerable<BlogPage> GetBlogPagesByDate(int? year, int? month, int? day, bool isPublished = true)
        {
            var blogPages = GetBlogPagesByDate(year, month, day);

            if (isPublished)
            {
                blogPages = blogPages.Where(b => b.IsPublished);
            }

            return blogPages.ToList();
        }

        /// <summary>
        /// Gets the blog pages by date.
        /// </summary>
        /// <param name="year">The year to search. If year is null, null is returned.</param>
        /// <param name="month">The month to search. Pass null to search by year.</param>
        /// <param name="day">The day to search. Pass null to search by month.</param>
        /// <returns>
        /// Returns a collection of blog pages published within the specified date range.
        /// </returns>
        public IQueryable<BlogPage> GetBlogPagesByDate(int? year, int? month, int? day)
        {
            int y = year ?? DateTime.MinValue.Year;
            int m = month ?? 0;
            int d = day ?? 0;

            if (y == DateTime.MinValue.Year && m == 0 && d == 0)
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

            return br.GetBlogPages().Where(b => b.PublishedDate > dateFrom && b.PublishedDate < dateTo && !b.IsContentPage);
        }

        public void AddBlogPage(BlogPage blogPage)
        {
                br.AddBlogPage(blogPage);            
        }

        public void UpdateBlogPage(BlogPage blogPage)
        {
                br.UpdateBlogPage(blogPage);
        }

        public void DeleteBlogPage(int id)
        {
                br.DeleteBlogPage(id);
        }
        #endregion
    }
}
