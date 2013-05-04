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
        #region Blog Settings Methods
        /// <summary>
        /// Gets the blog settings.
        /// </summary>
        /// <returns>Returns blog settings.</returns>
        BlogSettingsDto GetBlogSettings();

        /// <summary>
        /// Adds the settings if they do not exist or updates blog settings if they do.
        /// </summary>
        /// <param name="settings">The settings to add or update.</param>
        void AddOrUpdateBlogSettings(BlogSettingsDto settings);
        #endregion
    }
}
