using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;

using WR.Blog.Data.Contexts;
using WR.Blog.Data.Models;

namespace WR.Blog.Data.Repositories
{
    public class BlogRepository : IBlogRepository, IDisposable
    {
        #region Members
        /// <summary>
        /// Database context for connecting to blog database
        /// </summary>
        private readonly BlogDatabaseContext db; 
        #endregion

        public BlogRepository(BlogDatabaseContext db)
        {
            this.db = db;
        }

        #region Blog Methods
        /// <summary>
        /// Gets the blog post by id.
        /// </summary>
        /// <param name="id">The blog post id.</param>
        /// <returns>
        /// A blog post with the specified id.
        /// </returns>
        public BlogPostDto GetBlogPostById(int id)
        {
            return db.BlogPosts.Include("Author").Where(b => b.Id == id).SingleOrDefault();            
        }

        /// <summary>
        /// Gets all blog posts.
        /// </summary>
        /// <returns>Returns all blog posts.</returns>
        public IQueryable<BlogPostDto> GetBlogPosts()
        {
            return db.BlogPosts;
        }

        /// <summary>
        /// Adds the blog post.
        /// </summary>
        /// <param name="blogPost">The blog post to add.</param>
        public void AddBlogPost(BlogPostDto blogPost)
        {
            db.BlogPosts.Add(blogPost);
            db.SaveChanges();            
        }

        /// <summary>
        /// Updates the blog post.
        /// </summary>
        /// <param name="blogPost">The blog post to update.</param>
        public void UpdateBlogPost(BlogPostDto blogPost)
        {
            db.Entry(blogPost).State = EntityState.Modified;
            db.SaveChanges();             
        }

        /// <summary>
        /// Deletes the blog post.
        /// </summary>
        /// <param name="id">The id of the blog post to delete.</param>
        public void DeleteBlogPost(int id)
        {
            BlogPostDto blogpost = db.BlogPosts.Find(id);
            db.BlogPosts.Remove(blogpost);
            db.SaveChanges();            
        }
        #endregion

        #region Blog Version Methods
        /// <summary>
        /// Gets the version by id.
        /// </summary>
        /// <param name="id">The blog post version id.</param>
        /// <returns>
        /// The version with the specified id.
        /// </returns>
        public BlogVersionDto GetBlogPostVersionById(int id)
        {
            return db.BlogVersions.Find(id);
        }

        /// <summary>
        /// Adds the blog post version.
        /// </summary>
        /// <param name="version">The blog post version to add.</param>
        /// <returns>
        /// Returns the id of the blog post version.
        /// </returns>
        public int AddBlogPostVersion(BlogVersionDto version)
        {
            db.Entry(version.VersionOf).State = EntityState.Unchanged;
            db.BlogVersions.Add(version);
            db.SaveChanges();

            return version.Id;
        }

        /// <summary>
        /// Gets all blog post versions.
        /// </summary>
        /// <returns>
        /// Returns all blog post versions
        /// </returns>
        public IQueryable<BlogVersionDto> GetBlogPostVersions()
        {
            return db.BlogVersions;
        }

        /// <summary>
        /// Updates and saves the blog post version.
        /// </summary>
        /// <param name="version">The version to update.</param>
        public void UpdateBlogPostVersion(BlogVersionDto version)
        {
            db.Entry(version).State = EntityState.Modified;
            db.SaveChanges();
        }

        /// <summary>
        /// Deletes the blog post version.
        /// </summary>
        /// <param name="id">The id of the version to delete.</param>
        public void DeleteBlogPostVersion(int id)
        {
            BlogVersionDto version = db.BlogVersions.Find(id);
            db.BlogVersions.Remove(version);
            db.SaveChanges(); 
        }

        /// <summary>
        /// Deletes all versions for this blog post.
        /// </summary>
        /// <param name="id">The blog post id.</param>
        public void DeleteAllBlogPostVersions(int id)
        {
            db.BlogVersions.Where(v => v.VersionOf.Id == id)
                .ToList().ForEach(v => db.BlogVersions.Remove(v));
            db.SaveChanges();
        }
        #endregion

        #region Site Settings Methods
        /// <summary>
        /// Gets the site settings.
        /// </summary>
        /// <returns>
        /// Returns site setttings.
        /// </returns>
        public SiteSettingsDto GetSiteSettings()
        {
            return db.SiteSettings.FirstOrDefault();
        }

        /// <summary>
        /// Adds the settings if they do not exist or updates site settings if they do.
        /// </summary>
        /// <param name="settings">The settings to add or update.</param>
        public void AddOrUpdateSiteSettings(SiteSettingsDto settings)
        {
            if (settings.Id != 0)
            {
                db.Entry(settings).State = EntityState.Modified;
            }
            else
            {
                db.SiteSettings.Add(settings);
            }

            db.SaveChanges();
        }
        #endregion

        #region User Profile Methods
        /// <summary>
        /// Gets the user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        /// Returns the user profile or null if not found.
        /// </returns>
        public UserProfileDto GetUserByUsername(string username)
        {
            return db.UserProfiles.Where(u => u.UserName == username).FirstOrDefault();
        }
        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
