using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WR.Blog.Business.Services;

namespace WR.Blog.Mvc.Controllers
{
    public class HomeController : CommonController
    {
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
