using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WR.Blog.Mvc.Areas.SiteAdmin.Models
{
    public class BlogSettings
    {
        public int Id { get; set; }

        [StringLength(100)]
        [DisplayName("Site Title")]
        public string SiteTitle { get; set; }

        [StringLength(500)]
        [DisplayName("Tagline")]
        public string Tagline { get; set; }

        [StringLength(500)]
        [DisplayName("Alternate Tagline")]
        public string AltTagline { get; set; }

        [StringLength(1000)]
        [DisplayName("Menu Links")]
        public string MenuLinks { get; set; }

        [DefaultValue(10)]
        [DisplayName("Blog Posts Per Page")]
        public int PostsPerPage { get; set; }

        [DefaultValue(true)]
        [DisplayName("Allow Comments")]
        public bool AllowComments { get; set; }

        [DefaultValue(true)]
        [DisplayName("Moderate Comments")]
        public bool ModerateComments { get; set; }

        [StringLength(20)]
        [DisplayName("Google Analytics Account Number")]
        public string GoogleAccount { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("Date Last Updated")]
        public DateTime LastModifiedDate { get; set; }

        public string GravatarUrl { get; set; }

        public bool IsAdmin { get; set; }
    }
}