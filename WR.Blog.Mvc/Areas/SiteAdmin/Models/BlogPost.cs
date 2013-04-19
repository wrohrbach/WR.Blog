using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WR.Blog.Mvc.Areas.SiteAdmin.Models
{
    public class BlogPost
    {
        public BlogPost()
        {
            PublishedDate = DateTime.Now;
            IsPublished = false;
            AllowComments = true;
        }

        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; }

        [StringLength(100)]
        [DisplayName("URL Slug")]
        public string UrlSegment { get; set; }

        [Required]
        [DisplayName("Post Content")]
        public string Text { get; set; }

        public string AboveTheFold
        {
            get
            {
                return Text.Split(new string[] { "<!--more-->" }, StringSplitOptions.None).First();
            }
        }

        public bool IsSummarized
        {
            get
            {
                return Text.IndexOf("<!--more-->", StringComparison.OrdinalIgnoreCase) >= 0;
            }
        }

        public string Author { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("Date Last Modified")]
        public DateTime LastModifiedDate { get; set; }

        [Required]
        [DisplayName("Published?")]
        public bool IsPublished { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("Publish Date")]
        public DateTime PublishedDate { get; set; }

        [Required]
        [DisplayName("Comments?")]
        public bool AllowComments { get; set; }

        [Required]
        [DisplayName("Content Page?")]
        public bool IsContentPage { get; set; }
    }
}