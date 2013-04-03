using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WR.Blog.Mvc.Areas.SiteAdmin
{
    public class SiteAdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "SiteAdmin"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SiteAdmin_default",
                "SiteAdmin/{controller}/{action}/{id}",
                new { controller = "AdminHome", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "WR.Blog.Mvc.Areas.SiteAdmin.Controllers"}
            );
        }
    }
}