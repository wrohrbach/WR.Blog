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
        public DbSet<UserProfileDto> UserProfiles { get; set; }
        public DbSet<BlogSettingsDto> SiteSettings { get; set; }
        public DbSet<BlogPostDto> BlogPosts { get; set; }
        public DbSet<BlogVersionDto> BlogVersions { get; set; }
        public DbSet<BlogCommentDto> BlogComments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogPostDto>().Map(e =>
            {
                e.MapInheritedProperties();
                e.ToTable("BlogPosts");
            });

            modelBuilder.Entity<BlogVersionDto>().Map(e =>
            {
                e.MapInheritedProperties();
                e.ToTable("BlogVersions");
            });

            modelBuilder.Entity<BlogCommentDto>().Map(e =>
            {
                e.ToTable("BlogComments");
            });

            modelBuilder.Entity<BlogSettingsDto>().Map(e =>
            {
                e.ToTable("BlogSettings");
            });

            modelBuilder.Entity<UserProfileDto>().Map(e =>
            {
                e.ToTable("UserProfile");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}