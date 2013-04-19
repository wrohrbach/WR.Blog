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
    public class BlogVersionDto : BlogPostBaseDto
    {
        public BlogVersionDto()
            : base()
        { }

        public BlogVersionDto(BlogPostDto blogPage)
            : base()
        {
            
        }
        
        public virtual BlogPostDto VersionOf { get; set; }
    }
}