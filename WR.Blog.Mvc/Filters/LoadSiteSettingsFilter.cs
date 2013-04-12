using System.Web.Mvc;
using WR.Blog.Business.Services;
using WR.Blog.Data.Models;

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
            SiteSettings settings = manager.GetSiteSettings();

            var viewBag = filterContext.Controller.ViewBag;

            if (settings == null)
            {
                viewBag.SiteSettings = new SiteSettings{
                    SiteTitle = "Site Title",
                    Tagline = "Your blog's tagline.",
                    AltTagline = "Alternate Tagline",
                    MenuLinks = "",
                    PostsPerPage = 10,
                    AllowComments = true,
                    GoogleAccount = ""
                };
            }
            else
            {
                viewBag.SiteSettings = settings;
            }

            viewBag.IsAdmin = filterContext.HttpContext.User.IsInRole(System.Configuration.ConfigurationManager.AppSettings["AdministratorRole"]);

            base.OnActionExecuting(filterContext);
        }
    }
}