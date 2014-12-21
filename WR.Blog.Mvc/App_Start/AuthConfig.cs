using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Security;
using WebMatrix.WebData;

using WR.Blog.Data.Contexts;
using WR.Blog.Data.Models;

namespace WR.Blog.Mvc
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "",
            //    appSecret: "");

            //OAuthWebSecurity.RegisterGoogleClient();

            InitializeSimpleMembership();
        }

        private static void InitializeSimpleMembership()
        {
            Database.SetInitializer<BlogDatabaseContext>(null);

            try
            {
                using (var context = new BlogDatabaseContext())
                {
                    if (!context.Database.Exists())
                    {
                        // Create the SimpleMembership database without Entity Framework migration schema
                        ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                    }
                }

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
            catch (Exception ex)
            {
                throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
            }
        }
    }
}
