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
    public class BlogCommentDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual BlogPostDto BlogPost { get; set; }

        public virtual BlogCommentDto ReplyToComment { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(100)]
        [DisplayName("E-mail")]
        [RegularExpression(@"^([\w\!\#$\%\&\'\*\+\-\/\=\?\^\`{\|\}\~]+\.)*[\w\!\#$\%\&\'\*\+\-\/\=\?\^\`{\|\}\~]+@((((([a-zA-Z0-9]{1}[a-zA-Z0-9\-]{0,62}[a-zA-Z0-9]{1})|[a-zA-Z])\.)+[a-zA-Z]{2,6})|(\d{1,3}\.){3}\d{1,3}(\:\d{1,5})?)$")] 
        public string Email { get; set; }

        public string GravatarHash { get; set; }

        [StringLength(100)]
        [DisplayName("Home page")]
        public string Homepage { get; set; }

        [StringLength(100)]
        [DisplayName("Comment Title")]
        public string Title { get; set; }

        [Required]
        [DisplayName("Comment")]
        public string Comment { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("Date of Comment")]
        public DateTime CommentDate { get; set; }

        [StringLength(50)]
        [DisplayName("IP Address")]
        public string IpAddress { get; set; }

        [DisplayName("Approve for Publishing?")]
        public bool IsApproved { get; set; }

        [DisplayName("Deleted?")]
        public bool IsDeleted { get; set; }
    }
}
