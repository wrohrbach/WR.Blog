using System.Web.Mvc;
using WR.Blog.Business.Services;
using WR.Blog.Data.Models;
using WR.Blog.Mvc.Areas.SiteAdmin.Models;
using AutoMapper;

namespace WR.Blog.Mvc.Filters
{
    public class LoadSiteSettingsFilter : ActionFilterAttribute
    {
        private readonly ISiteManagerService manager;

        public LoadSiteSettingsFilter()
        { }

        public LoadSiteSettingsFilter(ISiteManagerService manager)
        {
            this.manager = manager;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            BlogSettingsDto settingsDto = manager.GetBlogSettings();
            var settings = Mapper.Map<BlogSettings>(settingsDto);

            var viewBag = filterContext.Controller.ViewBag;

            if (settings == null)
            {
                settings = new BlogSettings{
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

            settings.GravatarUrl = System.Configuration.ConfigurationManager.AppSettings["GravatarUrl"];
            settings.IsAdmin = filterContext.HttpContext.User.IsInRole(System.Configuration.ConfigurationManager.AppSettings["AdministratorRole"]);

            viewBag.BlogSettings = settings;

            base.OnActionExecuting(filterContext);
        }
    }
}