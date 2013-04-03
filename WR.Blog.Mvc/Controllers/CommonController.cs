using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WR.Blog.Data.Contexts;
using WR.Blog.Data.Models;
using WR.Blog.Business.Services;
using Ninject;
using WR.Blog.Data.Repositories;
using WR.Blog.Mvc.Filters;

namespace WR.Blog.Mvc.Controllers
{
    public class CommonController : Controller, IDisposable
    {
        protected BlogDatabaseContext db = new BlogDatabaseContext();
        
        public CommonController()
        { }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }       
    }
}
