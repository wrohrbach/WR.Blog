﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WR.Blog.Data.Models;

namespace WR.Blog.Data.Repositories
{
    public interface ISiteSettingsRepository : IDisposable
    {
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
    }
}