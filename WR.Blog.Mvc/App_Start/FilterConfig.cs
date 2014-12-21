using System.Web;
using System.Web.Mvc;
using WR.Blog.Mvc.Filters;

namespace WR.Blog.Mvc
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new LoadSiteSettingsFilter());
        }
    }
}