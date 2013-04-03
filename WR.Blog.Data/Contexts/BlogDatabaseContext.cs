using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using WR.Blog.Data.Models;

namespace WR.Blog.Data.Contexts
{
    public class BlogDatabaseContext : DbContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<BlogPage> BlogPages { get; set; }
        public DbSet<SiteSettings> SiteSettings { get; set; }
    }
}