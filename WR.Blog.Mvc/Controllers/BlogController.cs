using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WR.Blog.Data.Models;
using WR.Blog.Mvc.Helpers;
using WR.Blog.Business.Services;
using AutoMapper;
using WR.Blog.Mvc.Areas.SiteAdmin.Models;

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
                var blogPostDtos = blogger.GetBlogPostsByDate(year, month, day, true);

                // Check for issues getting pages by date
                if (blogPostDtos == null)
                {
                    throw new HttpException(404, "Not found");
                }

                var blogPosts = Mapper.Map<IEnumerable<BlogPostDto>, List<BlogPost>>(blogPostDtos);

                return View("List", blogPosts);
            }

            // Get permalinked blog post
            var blogPostDto = blogger.GetBlogPostByPermalink(year, month, day, urlSegment, isPublished: !(bool)ViewBag.IsAdmin);

            if (blogPostDto == null)
            {
                throw new HttpException(404, "Not found");
            }

            var blogPost = Mapper.Map<BlogPostDto, BlogPost>(blogPostDto);

            return View(blogPost);
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.Client,
                     Duration = 600)]
        public ActionResult ContentPage(string urlSegment)
        {
            if (string.IsNullOrWhiteSpace(urlSegment))
            {
                return List(1);
            }

            var blogPostDto = blogger.GetBlogPostByUrlSegment(urlSegment, isContentPage: true);

            if (blogPostDto == null || !(ViewBag.IsAdmin || (blogPostDto.IsPublished && blogPostDto.PublishedDate < DateTime.Now)))
            {
                throw new HttpException(404, "Not found");
            }

            var blogPost = Mapper.Map<BlogPostDto, BlogPost>(blogPostDto);

            return View(blogPost);
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.Client,
                     Duration = 600,
                     VaryByParam = "page")]
        public ActionResult List(int? page)
        {
            // TODO: Paging for blog list.
            var blogPostDtos = blogger.GetBlogPosts(page, null, published: true);

            var blogPosts = Mapper.Map<IEnumerable<BlogPostDto>, List<BlogPost>>(blogPostDtos);

            return View("List", blogPosts);
        }
    }
}