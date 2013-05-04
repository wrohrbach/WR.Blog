namespace WR.Blog.Data.Migrations
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
            string administratorRole = System.Configuration.ConfigurationManager.AppSettings["AdministratorRole"];
            string defaultUser = System.Configuration.ConfigurationManager.AppSettings["DefaultUser"];
            string defaultPassword = System.Configuration.ConfigurationManager.AppSettings["DefaultPassword"];

            WebSecurity.InitializeDatabaseConnection("BlogDatabaseContext", "UserProfile", "UserId", "UserName", autoCreateTables: true);

            if (!Roles.RoleExists(administratorRole))
            {
                Roles.CreateRole(administratorRole);
            }

            if (!WebSecurity.UserExists(defaultUser))
            {
                WebSecurity.CreateUserAndAccount(defaultUser, defaultPassword, new { DisplayName = defaultUser, Active = true, EmailAddress = "" });
            }

            if (!Roles.GetRolesForUser(defaultUser).Contains(administratorRole))
            {
                Roles.AddUsersToRoles(new[] { defaultUser }, new[] { administratorRole });
            }
        }
    }
}
