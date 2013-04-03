using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WR.Blog.Data.Models;
using WR.Blog.Data.Contexts;
using System.Data;

namespace WR.Blog.Data.Repositories
{
    public class SiteSettingsRepository : ISiteSettingsRepository
    {
        #region Members
        /// <summary>
        /// Database context for connecting to blog database
        /// </summary>
        private BlogDatabaseContext db;
        #endregion

        public SiteSettingsRepository(BlogDatabaseContext db)
        {
            this.db = db;
        }

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

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
                db.Dispose();
        }
    }
}
