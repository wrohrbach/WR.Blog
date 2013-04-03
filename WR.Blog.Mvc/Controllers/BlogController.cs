using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WR.Blog.Data.Models;
using WR.Blog.Mvc.Helpers;
using WR.Blog.Business.Services;

namespace WR.Blog.Mvc.Controllers
{
    public class BlogController : CommonController
    {
        private readonly IBlogService blogger;

        public BlogController(IBlogService blogger)
        {
            this.blogger = blogger;
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.Client,
                     Duration = 600)]
        public ActionResult Index(int? year, int? month, int? day, string urlSegment)
        {
            // If no parameter passed in, send to blog list.
            if (!(year.HasValue || month.HasValue || day.HasValue || !string.IsNullOrWhiteSpace(urlSegment)))
            {
                return RedirectToAction("List");
            }

            if (string.IsNullOrWhiteSpace(urlSegment))
            {
                var blogPages = blogger.GetBlogPagesByDate(year, month, day, true);

                // Check for issues getting pages by date
                if (blogPages == null)
                {
                    throw new HttpException(404, "Not found");
                }

                return View("List", blogPages);
            }

            // Get permalinked blog post
            var blogPage = blogger.GetBlogPageByPermalink(year, month, day, urlSegment, isPublished: !(bool)ViewBag.IsAdmin);

            if (blogPage == null)
            {
                throw new HttpException(404, "Not found");
            }

            return View(blogPage);
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.Client,
                     Duration = 600)]
        public ActionResult ContentPage(string urlSegment)
        {
            if (string.IsNullOrWhiteSpace(urlSegment))
            {
                return List(1);
            }

            var blogPage = blogger.GetBlogPageByUrlSegment(urlSegment, isContentPage: true);

            if (blogPage == null || !(ViewBag.IsAdmin || (blogPage.IsPublished && blogPage.PublishedDate < DateTime.Now)))
            {
                throw new HttpException(404, "Not found");
            }

            return View(blogPage);
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.Client,
                     Duration = 600,
                     VaryByParam = "page")]
        public ActionResult List(int? page)
        {
            // TODO: Paging for blog list.
            var blogPages = blogger.GetBlogPages(page, null, published: true);

            return View("List", blogPages);
        }
    }
}