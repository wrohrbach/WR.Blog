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
    public class BlogPostBaseDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; }

        [StringLength(100)]
        [DisplayName("URL Slug")]
        public string UrlSegment { get; set; }

        [Required]
        [DisplayName("Post Content")]
        public string Text { get; set; }

        public virtual UserProfileDto Author { get; set; }

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