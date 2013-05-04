using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;

using WR.Blog.Data.Contexts;
using WR.Blog.Data.Models;

namespace WR.Blog.Data.Repositories
{
    public class BlogRepository : IBlogRepository, IDisposable
    {
        #region Members
        /// <summary>
        /// Database context for connecting to blog database
        /// </summary>
        private readonly BlogDatabaseContext db; 
        #endregion

        public BlogRepository(BlogDatabaseContext db)
        {
            this.db = db;
        }

        #region Blog Methods
        /// <summary>
        /// Gets the blog post by id.
        /// </summary>
        /// <param name="blogPostId">The blog post id.</param>
        /// <returns>
        /// A blog post with the specified id.
        /// </returns>
        public BlogPostDto GetBlogPostById(int blogPostId)
        {
            return db.BlogPosts.Include("Author").Where(b => b.Id == blogPostId).SingleOrDefault();            
        }

        /// <summary>
        /// Gets all blog posts.
        /// </summary>
        /// <returns>Returns all blog posts.</returns>
        public IQueryable<BlogPostDto> GetBlogPosts()
        {
            return db.BlogPosts;
        }

        /// <summary>
        /// Adds the blog post.
        /// </summary>
        /// <param name="blogPost">The blog post to add.</param>
        public void AddBlogPost(BlogPostDto blogPost)
        {
            db.BlogPosts.Add(blogPost);
            db.SaveChanges();            
        }

        /// <summary>
        /// Updates the blog post.
        /// </summary>
        /// <param name="blogPost">The blog post to update.</param>
        public void UpdateBlogPost(BlogPostDto blogPost)
        {
            db.Entry(blogPost).State = EntityState.Modified;
            db.SaveChanges();             
        }

        /// <summary>
        /// Deletes the blog post.
        /// </summary>
        /// <param name="blogPostId">The id of the blog post to delete.</param>
        public void DeleteBlogPost(int blogPostId)
        {
            BlogPostDto blogpost = db.BlogPosts.Find(blogPostId);

            if (blogpost != null)
            {
                db.BlogPosts.Remove(blogpost);
                db.SaveChanges();             
            }
        }
        #endregion

        #region Blog Version Methods
        /// <summary>
        /// Gets the version by id.
        /// </summary>
        /// <param name="blogVersionId">The blog post version id.</param>
        /// <returns>
        /// The version with the specified id.
        /// </returns>
        public BlogVersionDto GetBlogPostVersionById(int blogVersionId)
        {
            return db.BlogVersions.Find(blogVersionId);
        }

        /// <summary>
        /// Adds the blog post version.
        /// </summary>
        /// <param name="version">The blog post version to add.</param>
        /// <returns>
        /// Returns the id of the blog post version.
        /// </returns>
        public int AddBlogPostVersion(BlogVersionDto version)
        {
            db.Entry(version.VersionOf).State = EntityState.Unchanged;
            db.BlogVersions.Add(version);
            db.SaveChanges();

            return version.Id;
        }

        /// <summary>
        /// Gets all blog post versions.
        /// </summary>
        /// <returns>
        /// Returns all blog post versions
        /// </returns>
        public IQueryable<BlogVersionDto> GetBlogPostVersions()
        {
            return db.BlogVersions;
        }

        /// <summary>
        /// Updates and saves the blog post version.
        /// </summary>
        /// <param name="version">The version to update.</param>
        public void UpdateBlogPostVersion(BlogVersionDto version)
        {
            db.Entry(version).State = EntityState.Modified;
            db.SaveChanges();
        }

        /// <summary>
        /// Deletes the blog post version.
        /// </summary>
        /// <param name="blogVersionId">The id of the version to delete.</param>
        public void DeleteBlogPostVersion(int blogVersionId)
        {
            BlogVersionDto version = db.BlogVersions.Find(blogVersionId);
            
            if (version != null)
            {
                db.BlogVersions.Remove(version);
                db.SaveChanges(); 
            } 
        }

        /// <summary>
        /// Deletes all versions for this blog post.
        /// </summary>
        /// <param name="blogPostId">The blog post id.</param>
        public void DeleteAllBlogPostVersions(int blogPostId)
        {
            db.BlogVersions.Where(v => v.VersionOf.Id == blogPostId)
                .ToList().ForEach(v => db.BlogVersions.Remove(v));
            db.SaveChanges();
        }
        #endregion

        #region Blog Comment Methods
        /// <summary>
        /// Gets the blog comment by id.
        /// </summary>
        /// <param name="blogCommentId">The blog comment id.</param>
        /// <returns>
        /// The comment with the specified id.
        /// </returns>
        public BlogCommentDto GetBlogCommentById(int blogCommentId)
        {
            return db.BlogComments.Find(blogCommentId);
        }

        /// <summary>
        /// Gets all blog comments.
        /// </summary>
        /// <returns>
        /// Returns all blog comments
        /// </returns>
        public IQueryable<BlogCommentDto> GetBlogComments()
        {
            return db.BlogComments;
        }

        /// <summary>
        /// Adds the blog comment.
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>
        /// Returns the id of the blog comment.
        /// </returns>
        public int AddBlogComment(BlogCommentDto comment)
        {
            db.Entry(comment.BlogPost).State = EntityState.Unchanged;

            if (comment.ReplyToComment != null)
            {
                db.Entry(comment.ReplyToComment).State = EntityState.Unchanged;
            }

            db.BlogComments.Add(comment);
            db.SaveChanges();

            return comment.Id;
        }

        /// <summary>
        /// Updates and saves the blog comment.
        /// </summary>
        /// <param name="comment"></param>
        public void UpdateBlogComment(BlogCommentDto comment)
        {
            db.Entry(comment).State = EntityState.Modified;
            db.SaveChanges();
        }

        /// <summary>
        /// Deletes the blog comment.
        /// </summary>
        /// <param name="blogCommentId">The id of the comment to delete.</param>
        public void DeleteBlogComment(int blogCommentId)
        {
            BlogCommentDto comment = db.BlogComments.Find(blogCommentId);

            if (comment != null)
            {
                db.BlogComments.Remove(comment);
                db.SaveChanges();
            } 
        }

        /// <summary>
        /// Deletes all comments for this blog post.
        /// </summary>
        /// <param name="blogPostId">The blog post id.</param>
        public void DeleteAllBlogPostComments(int blogPostId)
        {
            db.BlogComments.Where(c => c.BlogPost.Id == blogPostId)
                .ToList().ForEach(c => db.BlogComments.Remove(c));
            db.SaveChanges();
        } 
        #endregion

        #region Blog Settings Methods
        /// <summary>
        /// Gets the blog settings.
        /// </summary>
        /// <returns>
        /// Returns site setttings.
        /// </returns>
        public BlogSettingsDto GetBlogSettings()
        {
            return db.SiteSettings.FirstOrDefault();
        }

        /// <summary>
        /// Adds the settings if they do not exist or updates blog settings if they do.
        /// </summary>
        /// <param name="settings">The settings to add or update.</param>
        public void AddOrUpdateBlogSettings(BlogSettingsDto settings)
        {
            if (settings.Id != 0)
            {
                db.Entry(settings).State = EntityState.Modified;
            }
            else
            {
                db.SiteSettings.Add(settings);
            }

            db.SaveChanges();
        }
        #endregion

        #region User Profile Methods
        /// <summary>
        /// Gets the user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        /// Returns the user profile or null if not found.
        /// </returns>
        public UserProfileDto GetUserByUsername(string username)
        {
            return db.UserProfiles.Where(u => u.UserName == username).FirstOrDefault();
        }
        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
