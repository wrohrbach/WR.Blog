using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WR.Blog.Mvc.Models
{
    public class BlogComment
    {
        public int Id { get; set; }

        public int ReplyToCommentId { get; set; }

        public int BlogPostId { get; set; }

        public DateTime BlogPostPublishedDate { get; set; }

        public string BlogPostUrlSegment { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(100)]
        [DisplayName("E-mail")]
        [RegularExpression(@"^([\w\!\#$\%\&\'\*\+\-\/\=\?\^\`{\|\}\~]+\.)*[\w\!\#$\%\&\'\*\+\-\/\=\?\^\`{\|\}\~]+@((((([a-zA-Z0-9]{1}[a-zA-Z0-9\-]{0,62}[a-zA-Z0-9]{1})|[a-zA-Z])\.)+[a-zA-Z]{2,6})|(\d{1,3}\.){3}\d{1,3}(\:\d{1,5})?)$", 
            ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        public string GravatarHash { get; set; }

        [StringLength(100)]
        [DisplayName("Homepage")]
        public string Homepage { get; set; }

        [StringLength(100)]
        [DisplayName("Title")]
        public string Title { get; set; }

        [Required]
        [DisplayName("Comment")]
        public string Comment { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("Date of Comment"), DisplayFormat(DataFormatString = "{0: dddd, MMMM d, yyyy} at {0:h:mm tt}")]
        public DateTime CommentDate { get; set; }

        [DisplayName("Approved?")]
        public bool IsApproved { get; set; }
    }
}