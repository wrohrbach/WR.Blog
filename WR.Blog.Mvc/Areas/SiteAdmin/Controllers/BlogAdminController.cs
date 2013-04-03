using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WR.Blog.Mvc.Helpers;
using WR.Blog.Data.Models;
using WR.Blog.Business.Services;

namespace WR.Blog.Mvc.Areas.SiteAdmin.Controllers
{
    public class BlogAdminController : SiteAdminBaseController
    {
        private readonly IBlogService blogger;

        public BlogAdminController(IBlogService blogger)
        {
            this.blogger = blogger;
        }

        //
        // GET: /SiteAdmin/Blog/

        public ActionResult Index(int? page)
        {
            return View(blogger.GetBlogPages(page, null, content: true));
        }

        //
        // GET: /SiteAdmin/Blog/Create

        public ActionResult Create()
        {
            var blogpage = new BlogPage();

            return View(blogpage);
        }

        //
        // POST: /SiteAdmin/Blog/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(BlogPage blogPage)
        {
            if (ModelState.IsValid)
            {
                var user = blogger.GetUser(User.Identity.Name);

                if (user == null)
                {
                    return new HttpUnauthorizedResult();
                }

                blogPage.Author = user;
                blogPage.LastModifiedDate = DateTime.Now;
                blogPage.UrlSegment = blogPage.Title.Clean();

                blogger.AddBlogPage(blogPage);

                if (blogPage.IsContentPage)
                {
                    return RedirectToRoute("Content", routeValues: new { Area = string.Empty, urlSegment = blogPage.UrlSegment });
                }

                return RedirectToRoute("BlogPage", routeValues: new
                    {
                        Area = string.Empty,
                        year = blogPage.PublishedDate.Year,
                        month = blogPage.PublishedDate.Month,
                        day = blogPage.PublishedDate.Day,
                        urlSegment = blogPage.UrlSegment
                    });
            }

            return View(blogPage);
        }

        //
        // GET: /SiteAdmin/Blog/Edit/5

        public ActionResult Edit(int id = 0)
        {
            BlogPage blogpage = blogger.GetBlogPage(id);

            if (blogpage == null)
            {
                return HttpNotFound();
            }

            return View(blogpage);
        }

        //
        // POST: /SiteAdmin/Blog/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(BlogPage blogPage)
        {
            if (ModelState.IsValid)
            {
                var user = blogger.GetUser(User.Identity.Name);

                if (user == null)
                {
                    return new HttpUnauthorizedResult();
                }

                blogPage.Author = user;
                blogPage.LastModifiedDate = DateTime.Now;
                blogPage.UrlSegment = blogPage.Title.Clean();

                blogger.UpdateBlogPage(blogPage);

                if (blogPage.IsContentPage)
                {
                    return RedirectToRoute("Content", routeValues: new { Area = string.Empty, urlSegment = blogPage.UrlSegment });
                }

                return RedirectToRoute("BlogPage", routeValues: new
                    {
                        Area = string.Empty,
                        year = blogPage.PublishedDate.Year,
                        month = blogPage.PublishedDate.Month,
                        day = blogPage.PublishedDate.Day,
                        urlSegment = blogPage.UrlSegment
                    });
            }

            return View(blogPage);
        }

        //
        // GET: /SiteAdmin/Blog/Delete/5

        public ActionResult Delete(int id = 0)
        {
            BlogPage blogpage = blogger.GetBlogPage(id);

            if (blogpage == null)
            {
                return HttpNotFound();
            }

            return View(blogpage);
        }

        //
        // POST: /SiteAdmin/Blog/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            blogger.DeleteBlogPage(id);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}