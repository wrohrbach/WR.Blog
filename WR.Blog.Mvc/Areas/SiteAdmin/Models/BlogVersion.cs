using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WR.Blog.Mvc.Areas.SiteAdmin.Models
{
    public class BlogVersion : BlogPost
    {
        public int VersionOfId { get; set; }
    }
}