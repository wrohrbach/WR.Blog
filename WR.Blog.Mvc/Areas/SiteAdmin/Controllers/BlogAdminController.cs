using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WR.Blog.Mvc.Areas.SiteAdmin.Models;
using WR.Blog.Mvc.Helpers;
using WR.Blog.Data.Models;
using WR.Blog.Business.Services;
using AutoMapper;
using WR.Blog.Mvc.Models;

namespace WR.Blog.Mvc.Areas.SiteAdmin.Controllers
{
    public class BlogAdminController : SiteAdminBaseController
    {
        private readonly IBlogService blogger;

        public BlogAdminController(IBlogService blogger)
        {
            this.blogger = blogger;
        }

        #region BlogPost Actions
        //
        // GET: /SiteAdmin/Blog/

        public ActionResult Index(int? page)
        {
            var blogPostDtos = blogger.GetBlogPosts(page, take: null, content: true);
            var blogPosts = Mapper.Map<IEnumerable<BlogPostDto>, List<BlogPost>>(blogPostDtos);

            return View(blogPosts);
        }

        //
        // GET: /SiteAdmin/Blog/Create

        public ActionResult Create()
        {
            var blogpost = new BlogPost();

            return View(blogpost);
        }

        //
        // POST: /SiteAdmin/Blog/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(string submitButton)
        {
            var blogPost = new BlogPost();
            TryUpdateModel(blogPost);

            if (ModelState.IsValid)
            {
                var blogPostDto = Mapper.Map<BlogPost, BlogPostDto>(blogPost);

                var user = blogger.GetUser(User.Identity.Name);

                if (user == null)
                {
                    return new HttpUnauthorizedResult();
                }

                blogPostDto.Author = user;
                blogPostDto.LastModifiedDate = DateTime.Now;
                blogPostDto.UrlSegment = blogPostDto.Title.Clean();

                if (submitButton == "Save and Preview")
                {
                    blogPostDto.IsPublished = false;
                }

                blogger.AddBlogPost(blogPostDto);

                if (blogPostDto.IsContentPage)
                {
                    return RedirectToRoute("Content", routeValues: new { Area = string.Empty, urlSegment = blogPostDto.UrlSegment });
                }

                return RedirectToRoute("BlogPost", routeValues: new
                    {
                        Area = string.Empty,
                        year = blogPostDto.PublishedDate.Year,
                        month = blogPostDto.PublishedDate.Month,
                        day = blogPostDto.PublishedDate.Day,
                        urlSegment = blogPostDto.UrlSegment
                    });
            }

            return View(blogPost);
        }

        //
        // GET: /SiteAdmin/Blog/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var blogPostDto = blogger.GetBlogPost(id);

            if (blogPostDto == null)
            {
                throw new HttpException(404, "Not found");
            }

            var blogPost = Mapper.Map<BlogPostDto, BlogPost>(blogPostDto);

            return View(blogPost);
        }

        //
        // POST: /SiteAdmin/Blog/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(string submitButton)
        {
            var blogPost = new BlogPost();
            TryUpdateModel(blogPost);
            
            if (ModelState.IsValid)
            {
                if (submitButton == "Publish Changes")
                {
                    // Save a version the original before any changes are made
                    blogger.SaveBlogPostAsVersion(blogPost.Id);
                }

                var blogPostDto = blogger.GetBlogPost(blogPost.Id);
                Mapper.Map<BlogPost, BlogPostDto>(blogPost, blogPostDto);

                var user = blogger.GetUser(User.Identity.Name);

                if (user == null)
                {
                    return new HttpUnauthorizedResult();
                }

                blogPostDto.Author = user;
                blogPostDto.LastModifiedDate = DateTime.Now;
                blogPostDto.UrlSegment = blogPostDto.Title.Clean();

                if (submitButton == "Save and Preview")
                {
                    int previewId = blogger.SaveBlogPostAsVersion(blogPostDto);

                    return RedirectToAction("PreviewVersion", routeValues: new { id = previewId });
                }

                blogger.UpdateBlogPost(blogPostDto);

                if (blogPostDto.IsContentPage)
                {
                    return RedirectToRoute("Content", routeValues: new { Area = string.Empty, urlSegment = blogPostDto.UrlSegment });
                }

                return RedirectToRoute("BlogPost", routeValues: new
                    {
                        Area = string.Empty,
                        year = blogPostDto.PublishedDate.Year,
                        month = blogPostDto.PublishedDate.Month,
                        day = blogPostDto.PublishedDate.Day,
                        urlSegment = blogPostDto.UrlSegment
                    });
            }

            return View(blogPost);
        }

        //
        // GET: /SiteAdmin/Blog/Delete/5

        public ActionResult Delete(int id = 0)
        {
            var blogPostDto = blogger.GetBlogPost(id);            

            if (blogPostDto == null)
            {
                throw new HttpException(404, "Not found");
            }

            var blogPost = Mapper.Map<BlogPostDto, BlogPost>(blogPostDto);

            return View(blogPost);
        }

        //
        // POST: /SiteAdmin/Blog/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            blogger.DeleteBlogPost(id);

            return RedirectToAction("Index");
        } 
        #endregion

        #region Version Actions
        public ActionResult Versions(int? id, int? page)
        {
            // TODO: Version paging
            int blogPostId = id ?? 0;

            var blogPostDto = blogger.GetBlogPost(blogPostId);

            if (blogPostDto != null)
            {
                ViewBag.BlogPostTitle = blogPostDto.Title;
            }

            var versionDtos = blogger.GetVersionsByBlogPost(blogPostId);
            var versions = Mapper.Map<IEnumerable<BlogVersionDto>, List<BlogVersion>>(versionDtos);

            return View(versions);
        }

        public ActionResult PreviewVersion(int? id)
        {
            int versionId = id ?? 0;

            var versionDto = blogger.GetVersion(versionId);

            if (versionDto == null)
            {
                throw new HttpException(404, "Not found");
            }

            var version = Mapper.Map<BlogVersionDto, BlogVersion>(versionDto);

            return View(version);
        }

        public ActionResult EditVersion(int id = 0)
        {
            var versionDto = blogger.GetVersion(id);

            if (versionDto == null)
            {
                throw new HttpException(404, "Not found");
            }

            var version = Mapper.Map<BlogVersionDto, BlogVersion>(versionDto);

            return View(version);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditVersion(string submitButton)
        {
            var version = new BlogVersion();
            TryUpdateModel(version);

            if (ModelState.IsValid)
            {
                var versionDto = blogger.GetVersion(version.Id);
                Mapper.Map<BlogVersion, BlogVersionDto>(version, versionDto);

                var user = blogger.GetUser(User.Identity.Name);

                if (user == null)
                {
                    return new HttpUnauthorizedResult();
                }

                versionDto.Author = user;
                versionDto.LastModifiedDate = version.LastModifiedDate = DateTime.Now;
                versionDto.UrlSegment = versionDto.Title.Clean();
                
                blogger.UpdateVersion(versionDto);

                switch (submitButton)
                {
                    case "Save and Preview":
                        {
                            return RedirectToAction("PreviewVersion", routeValues: new { id = versionDto.Id });
                        }
                    case "Save and Publish":
                        {
                            blogger.PublishVersion(versionDto);
                            
                            return RedirectToRoute("BlogPost", routeValues: new
                            {
                                Area = string.Empty,
                                year = versionDto.VersionOf.PublishedDate.Year,
                                month = versionDto.VersionOf.PublishedDate.Month,
                                day = versionDto.VersionOf.PublishedDate.Day,
                                urlSegment = versionDto.UrlSegment
                            });
                        }
                    default:
                        break;
                }

                ViewBag.Message = "Version Saved.";
                ViewBag.MessageStyle = "message-success";
            }

            return View(version);
        }

        [HttpPost]
        public ActionResult PublishVersion(int? id, string submitButton)
        {
            int versionId = id ?? 0;

            var versionDto = blogger.GetVersion(versionId);

            if (versionDto == null)
            {
                throw new HttpException(404, "Not found");
            }

            blogger.PublishVersion(versionDto);

            return RedirectToRoute("BlogPost", routeValues: new
            {
                Area = string.Empty,
                year = versionDto.VersionOf.PublishedDate.Year,
                month = versionDto.VersionOf.PublishedDate.Month,
                day = versionDto.VersionOf.PublishedDate.Day,
                urlSegment = versionDto.UrlSegment
            });
        }

        public ActionResult DeleteVersion(int id = 0)
        {
            var versionDto = blogger.GetVersion(id);

            if (versionDto == null)
            {
                throw new HttpException(404, "Not found");
            }

            var version = Mapper.Map<BlogVersionDto, BlogVersion>(versionDto);

            return View(version);
        }
        
        [HttpPost, ActionName("DeleteVersion")]
        public ActionResult DeleteVersionConfirmed(int id, int versionOfId)
        {
            blogger.DeleteVersion(id);

            return RedirectToAction("Versions", routeValues: new { id = versionOfId });
        } 
        #endregion

        #region Comment Actions
        public ActionResult Comments(int? id, int? page)
        {
            // TODO: Comment Paging
            int blogPostId = id ?? 0;

            if (blogPostId > 0)
            {
                var commentDtos = blogger.GetCommentsByBlogPost(blogPostId, isApproved: false, isDeleted: true);
                var comments = Mapper.Map<IEnumerable<BlogCommentDto>, List<BlogComment>>(commentDtos);

                return View(comments);
            }
            else
            {
                var commentDtos = blogger.GetUnapprovedComments();
                var comments = Mapper.Map<IEnumerable<BlogCommentDto>, List<BlogComment>>(commentDtos);

                return View("UnapprovedComments", comments);
            }
        }

        public ActionResult ViewComment(int? id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Approves the comment to display.
        /// </summary>
        /// <param name="id">The comment id.</param>
        /// <param name="isApproved">Set to false to unapprove a comment. Leave as true to approve.</param>
        /// <returns>
        /// Returns a JsonResult saying whether this was successfully approved or not.
        /// </returns>
        [HttpPost]
        public JsonResult ApproveComment(int id, bool isApproved = true)
        {
            blogger.ApproveComment(id, isApproved);

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult ApproveAllComments()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public JsonResult DeleteComment(int id)
        {
            blogger.DeleteComment(id);

            return Json(new { success = true });
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}