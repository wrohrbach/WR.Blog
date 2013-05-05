using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AutoMapper;
using PoliteCaptcha;
using WR.Blog.Business.Services;
using WR.Blog.Business.Helpers;
using WR.Blog.Data.Models;
using WR.Blog.Mvc.Helpers;
using WR.Blog.Mvc.Areas.SiteAdmin.Models;
using WR.Blog.Mvc.Models;
using System.Text.RegularExpressions;

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
            var blogPostDto = blogger.GetBlogPostByPermalink(year, month, day, urlSegment, isPublished: !ViewBag.BlogSettings.IsAdmin);

            if (blogPostDto == null)
            {
                throw new HttpException(404, "Not found");
            }

            var blogPost = Mapper.Map<BlogPostDto, BlogPost>(blogPostDto);
            blogPost.Comment = new BlogComment
            {
                BlogPostId = blogPost.Id,
                BlogPostPublishedDate = blogPost.PublishedDate,
                BlogPostUrlSegment = blogPost.UrlSegment
            };

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

            if (blogPostDto == null || !(ViewBag.BlogSettings.IsAdmin || (blogPostDto.IsPublished && blogPostDto.PublishedDate < DateTime.Now)))
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

        public PartialViewResult Comments(int? id)
        {
            int blogPostId = id ?? 0;

            var commentDtos = blogger.GetCommentsByBlogPost(blogPostId, ((BlogSettings)ViewBag.BlogSettings).ModerateComments);

            var comments = Mapper.Map<IEnumerable<BlogCommentDto>, List<BlogComment>>(commentDtos);

            return PartialView(comments);
        }

        [HttpPost, ValidateSpamPrevention]
        [ValidateInput(false)]
        public ActionResult AddComment()
        {
            BlogComment comment = new BlogComment();
            TryUpdateModel(comment);

            if (ModelState.IsValid)
            {
                BlogCommentDto commentDto = Mapper.Map<BlogCommentDto>(comment);

                if (comment.ReplyToCommentId != 0)
                {
                    commentDto.ReplyToComment = blogger.GetComment(comment.ReplyToCommentId);
                }

                commentDto.BlogPost = blogger.GetBlogPost(comment.BlogPostId);
                commentDto.CommentDate = DateTime.Now;
                commentDto.IsApproved = !ViewBag.BlogSettings.ModerateComments || ViewBag.BlogSettings.IsAdmin;
                commentDto.GravatarHash = commentDto.Email.GravatarHash();
                commentDto.Homepage = commentDto.Homepage.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? commentDto.Homepage : "http://" + commentDto.Homepage;
                commentDto.Comment = commentDto.Comment.ToSafeHtml();

                blogger.AddComment(commentDto);

                return Redirect(Url.RouteUrl(routeValues: new { 
                        controller = "Blog",
                        action = "Index",
                        year = comment.BlogPostPublishedDate.Year,
                        month = comment.BlogPostPublishedDate.Month,
                        day = comment.BlogPostPublishedDate.Day, 
                        urlSegment = comment.BlogPostUrlSegment
                    }) + "#comment" + commentDto.Id);

                //return RedirectToAction("Index", 
                //    routeValues: new { 
                //        year = comment.BlogPostPublishedDate.Year,
                //        month = comment.BlogPostPublishedDate.Month,
                //        day = comment.BlogPostPublishedDate.Day, 
                //        urlSegment = comment.BlogPostUrlSegment
                //    });
            }

            BlogPost blogPost = Mapper.Map<BlogPost>(blogger.GetBlogPost(comment.BlogPostId));
            blogPost.Comment = comment;

            ViewBag.MessageClass = "message-error";
            ViewBag.Message = "There was a problem with your comment. See the error message(s) <a href=\"#addcomment\">below</a>.";

            return View("Index", blogPost);
        }
    }
}