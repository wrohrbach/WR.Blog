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
                viewBag.SiteTitle = "Site Title";
                viewBag.Tagline = "Your blog's tagline.";
                viewBag.AlternateTagline = "Alternate Tagline";
                viewBag.MenuLinks = "";
            }
            else
            {
                viewBag.SiteTitle = settings.SiteTitle;
                viewBag.Tagline = settings.Tagline;
                viewBag.AltTagline = settings.AltTagline;
                viewBag.MenuLinks = settings.MenuLinks;
                viewBag.GoogleAccount = settings.GoogleAccount;
            }

            viewBag.IsAdmin = filterContext.HttpContext.User.IsInRole(System.Configuration.ConfigurationManager.AppSettings["AdministratorRole"]);

            base.OnActionExecuting(filterContext);
        }
    }
}