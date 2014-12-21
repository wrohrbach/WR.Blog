using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Ninject;
using WR.Blog.Business.Services;
using WR.Blog.Data.Models;
using WR.Blog.Mvc.Areas.SiteAdmin.Models;

namespace WR.Blog.Mvc.Config
{
    public sealed class SettingsManager
    {
        #region Private Member Variables
        private readonly ISiteManagerService manager;
        private static volatile SettingsManager instance;
        private static object syncRoot = new Object();
        #endregion

        #region Constructors
        private SettingsManager(ISiteManagerService manager)
        {
            this.manager = manager;
            var blogSettingsDto = this.manager.GetBlogSettings();
            Settings = Mapper.Map<Settings>(blogSettingsDto);

            if (Settings == null)
            {
                Settings = new Settings
                {
                    SiteTitle = "Site Title",
                    Tagline = "Your blog's tagline.",
                    AltTagline = "Alternate Tagline",
                    MenuLinks = "",
                    PostsPerPage = 10,
                    AllowComments = true,
                    ModerateComments = true,
                    GoogleAccount = ""
                };
            }

            Settings.GravatarUrl = System.Configuration.ConfigurationManager.AppSettings["GravatarUrl"];
        }
        #endregion

        #region Public Properties
        public static SettingsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            var kernerl = new StandardKernel(new SiteManagerSingletonModule());
                            instance = new SettingsManager(kernerl.Get<ISiteManagerService>());
                        }
                    }
                }

                return instance;
            }
        }

        public Settings Settings { get; private set; }
        #endregion

        #region Public Methods
        internal static void UpdateSettings(BlogSettings settingsModel)
        {
            var singleInstance = Instance;

            lock (syncRoot)
            {
                // Update singleton object's settings values
                Mapper.Map<BlogSettings, Settings>(settingsModel, singleInstance.Settings);
            }

        }
        #endregion
    }
}