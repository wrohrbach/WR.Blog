using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WR.Blog.Data.Models;

namespace WR.Blog.Data.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBlogRepository : IDisposable
    {
        #region Blog Methods
        /// <summary>
        /// Gets the blog post by id.
        /// </summary>
        /// <param name="id">The blog post id.</param>
        /// <returns>A blog post with the specified id.</returns>
        BlogPostDto GetBlogPostById(int id);

        /// <summary>
        /// Gets all blog posts.
        /// </summary>
        /// <returns>Returns all blog posts.</returns>
        IQueryable<BlogPostDto> GetBlogPosts();

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
        /// Gets the blog post version by id.
        /// </summary>
        /// <param name="id">The blog post version id.</param>
        /// <returns>The version with the specified id.</returns>
        BlogVersionDto GetBlogPostVersionById(int id);

        /// <summary>
        /// Gets all blog post versions.
        /// </summary>
        /// <returns>Returns all blog post versions</returns>
        IQueryable<BlogVersionDto> GetBlogPostVersions();

        /// <summary>
        /// Adds the blog post version.
        /// </summary>
        /// <param name="version">The blog post version to add.</param>
        /// <returns>Returns the id of the blog post version.</returns>
        int AddBlogPostVersion(BlogVersionDto version);

        /// <summary>
        /// Updates and saves the blog post version.
        /// </summary>
        /// <param name="version">The version to update.</param>
        void UpdateBlogPostVersion(BlogVersionDto version);

        /// <summary>
        /// Deletes the blog post version.
        /// </summary>
        /// <param name="id">The id of the version to delete.</param>
        void DeleteBlogPostVersion(int id);

        /// <summary>
        /// Deletes all versions for this blog post.
        /// </summary>
        /// <param name="id">The blog post id.</param>
        void DeleteAllBlogPostVersions(int id);
        #endregion

        #region Site Settings Methods
        /// <summary>
        /// Gets the site settings.
        /// </summary>
        /// <returns>Returns site setttings.</returns>
        SiteSettingsDto GetSiteSettings();

        /// <summary>
        /// Adds the settings if they do not exist or updates site settings if they do.
        /// </summary>
        /// <param name="settings">The settings to add or update.</param>
        void AddOrUpdateSiteSettings(SiteSettingsDto settings);
        #endregion

        #region User Profile Methods
        /// <summary>
        /// Gets the user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>Returns the user profile or null if not found.</returns>
        UserProfileDto GetUserByUsername(string username);
        #endregion
    }
}
