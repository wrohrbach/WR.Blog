using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WR.Blog.Data.Models;

namespace WR.Blog.Data.Repositories
{
    public interface IBlogRepository : IDisposable
    {
        #region Blog Methods
        /// <summary>
        /// Gets the blog page by id.
        /// </summary>
        /// <param name="id">The blog page id.</param>
        /// <returns>A blog page with the specified id.</returns>
        BlogPage GetBlogPageById(int id);

        /// <summary>
        /// Gets all blog pages.
        /// </summary>
        /// <returns>Returns all blog pages.</returns>
        IQueryable<BlogPage> GetBlogPages();

        void AddBlogPage(BlogPage blogPage);

        void UpdateBlogPage(BlogPage blogPage);

        void DeleteBlogPage(int id);
        #endregion

        #region Site Settings Methods
        /// <summary>
        /// Gets the site settings.
        /// </summary>
        /// <returns>Returns site setttings.</returns>
        SiteSettings GetSiteSettings();

        /// <summary>
        /// Adds the settings if they do not exist or updates site settings if they do.
        /// </summary>
        /// <param name="settings">The settings to add or update.</param>
        void AddOrUpdateSiteSettings(SiteSettings settings);
        #endregion

        #region User Profile Methods
        /// <summary>
        /// Gets the user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>Returns the user profile or null if not found.</returns>
        UserProfile GetUserByUsername(string username);
        #endregion
    }
}
