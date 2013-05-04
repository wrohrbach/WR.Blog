using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Text;

namespace WR.Blog.Data.Models
{
    public class BlogPostDto : BlogPostBaseDto
    {
        public BlogPostDto()
        {
            PublishedDate = DateTime.Now;
            IsPublished = false;
            AllowComments = true;
        }

        /// <summary>
        /// Gets or sets this blog post's comment collection.
        /// </summary>
        /// <value>
        /// The comments on this blog post.
        /// </value>
        public virtual ICollection<BlogCommentDto> Comments { get; set; }
    }
}