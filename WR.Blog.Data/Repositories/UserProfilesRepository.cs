using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WR.Blog.Data.Models;
using WR.Blog.Data.Contexts;
using System.Data;

namespace WR.Blog.Data.Repositories
{
    public class UserProfilesRepository : IUserProfilesRepository
    {
        #region User Pro Members
        /// <summary>
        /// Database context for connecting to blog database
        /// </summary>
        private BlogDatabaseContext db;
        #endregion

        public UserProfilesRepository(BlogDatabaseContext db)
        {
            this.db = db;
        }

        #region User Profile Methods
        /// <summary>
        /// Gets the user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        /// Returns the user profile or null if not found.
        /// </returns>
        public UserProfile GetUserByUsername(string username)
        {
            using (db = new BlogDatabaseContext())
            {
                return db.UserProfiles.Where(u => u.UserName == username).FirstOrDefault();
            }
        }        
        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (db != null)
            {
                db.Dispose();
            }
        }
    }
}
