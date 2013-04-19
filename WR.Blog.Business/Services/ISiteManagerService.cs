using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WR.Blog.Data.Models;
using WR.Blog.Data.Repositories;

namespace WR.Blog.Business.Services
{
    public interface ISiteManagerService : IBaseService, IDisposable
    {
        #region Site Settings Methods
        /// <summary>
        /// Gets the site settings.
        /// </summary>
        /// <returns>Returns site settings.</returns>
        SiteSettingsDto GetSiteSettings();

        /// <summary>
        /// Adds the settings if they do not exist or updates site settings if they do.
        /// </summary>
        /// <param name="settings">The settings to add or update.</param>
        void AddOrUpdateSiteSettings(SiteSettingsDto settings);
        #endregion
    }
}
