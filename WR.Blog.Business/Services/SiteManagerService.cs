using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WR.Blog.Data.Models;
using WR.Blog.Data.Repositories;

namespace WR.Blog.Business.Services
{
    public class SiteManagerService : BaseService, ISiteManagerService
    {
        public SiteManagerService(IBlogRepository br) : base(br)
        {
        }

        #region Site Settings Methods
        /// <summary>
        /// Gets the site settings.
        /// </summary>
        /// <returns>
        /// Returns site settings.
        /// </returns>
        public SiteSettingsDto GetSiteSettings()
        {
                return br.GetSiteSettings() ?? new SiteSettingsDto
                                                        {
                                                            Id = 0,
                                                            SiteTitle = "Your Site Title",
                                                            MenuLinks = "",
                                                            AllowComments = true,
                                                            PostsPerPage = 10
                                                        }; 
        }

        /// <summary>
        /// Adds the settings if they do not exist or updates site settings if they do.
        /// </summary>
        /// <param name="settings">The settings to add or update.</param>
        /// <exception cref="System.UnauthorizedAccessException">Throws unauthorized exception if user is not found.</exception>
        public void AddOrUpdateSiteSettings(SiteSettingsDto settings)
        {
                br.AddOrUpdateSiteSettings(settings); 
        }
        #endregion
    }
}
