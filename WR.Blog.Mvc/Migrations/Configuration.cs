namespace WR.Blog.Mvc.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebMatrix.WebData;
    using System.Web.Security;

    internal sealed class Configuration : DbMigrationsConfiguration<WR.Blog.Data.Contexts.BlogDatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WR.Blog.Data.Contexts.BlogDatabaseContext context)
        {
            WebSecurity.InitializeDatabaseConnection("BlogDatabaseContext", "UserProfile", "UserId", "UserName", autoCreateTables: true);

            if (!Roles.RoleExists("Administrator"))
            {
                Roles.CreateRole("Administrator");
            }

            if (!WebSecurity.UserExists("wade"))
            {
                WebSecurity.CreateUserAndAccount("wade", "soybean", new { DisplayName = "Wade", Active = true, EmailAddress = "wade.rohrbach@gmail.com" });
            }

            if (!Roles.GetRolesForUser("wade").Contains("Administrator"))
            {
                Roles.AddUsersToRoles(new[] { "wade" }, new[] { "Administrator" });
            }
        }
    }
}
