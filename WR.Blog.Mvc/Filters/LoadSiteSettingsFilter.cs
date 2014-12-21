using System.Web.Mvc;
//using WR.Blog.Business.Services;
using WR.Blog.Data.Models;
using WR.Blog.Mvc.Areas.SiteAdmin.Models;
using AutoMapper;
using WR.Blog.Mvc.Config;

namespace WR.Blog.Mvc.Filters
{
    public class LoadSiteSettingsFilter : ActionFilterAttribute
    {
        //private readonly ISiteManagerService manager;

        public LoadSiteSettingsFilter()
        { }

        //public LoadSiteSettingsFilter(ISiteManagerService manager)
        //{
        //    this.manager = manager;
        //}

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var viewBag = filterContext.Controller.ViewBag;

            if (viewBag.BlogSettings == null)
            {
                BlogSettings settings = Mapper.Map<BlogSettings>(SettingsManager.Instance.Settings);
                settings.IsAdmin = filterContext.HttpContext.User.IsInRole(System.Configuration.ConfigurationManager.AppSettings["AdministratorRole"]);

                viewBag.BlogSettings = settings; 
            }

            base.OnActionExecuting(filterContext);
        }
    }
}