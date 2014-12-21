using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WR.Blog.Mvc.Config
{
    public class Settings
    {
        public int Id { get; set; }

        public string SiteTitle { get; set; }

        public string Tagline { get; set; }

        public string AltTagline { get; set; }

        public string MenuLinks { get; set; }

        public int PostsPerPage { get; set; }

        public bool AllowComments { get; set; }

        public bool ModerateComments { get; set; }

        public string GoogleAccount { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public string GravatarUrl { get; set; }
    }
}