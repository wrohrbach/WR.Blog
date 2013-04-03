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
        /// Gets the blog page by id.
        /// </summary>
        /// <param name="id">The blog page id.</param>
        /// <returns>
        /// A blog page with the specified id.
        /// </returns>
        public BlogPage GetBlogPageById(int id)
        {
                return db.BlogPages.Include("Author").Where(b => b.Id == id).SingleOrDefault();            
        }

        /// <summary>
        /// Gets all blog pages.
        /// </summary>
        /// <returns>Returns all blog pages.</returns>
        public IQueryable<BlogPage> GetBlogPages()
        {
            return db.BlogPages;
        }

        public void AddBlogPage(BlogPage blogPage)
        {
                db.BlogPages.Add(blogPage);
                db.SaveChanges();            
        }

        public void UpdateBlogPage(BlogPage blogPage)
        {
                db.Entry(blogPage).State = EntityState.Modified;
                db.SaveChanges();             
        }

        public void DeleteBlogPage(int id)
        {
                BlogPage blogpage = db.BlogPages.Find(id);
                db.BlogPages.Remove(blogpage);
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
        public SiteSettings GetSiteSettings()
        {
            return db.SiteSettings.FirstOrDefault();
        }

        /// <summary>
        /// Adds the settings if they do not exist or updates site settings if they do.
        /// </summary>
        /// <param name="settings">The settings to add or update.</param>
        public void AddOrUpdateSiteSettings(SiteSettings settings)
        {
            if (settings.Id != 0)
            {
                var state = db.Entry(settings).State;

                db.SiteSettings.Attach(settings);
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
        public UserProfile GetUserByUsername(string username)
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
