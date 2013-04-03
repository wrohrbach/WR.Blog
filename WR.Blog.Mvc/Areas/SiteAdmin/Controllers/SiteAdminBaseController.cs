using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WR.Blog.Mvc.Controllers;
using WR.Blog.Business.Services;

namespace WR.Blog.Mvc.Areas.SiteAdmin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class SiteAdminBaseController : CommonController
    {
    }
}
