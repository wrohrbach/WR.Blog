using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WR.Blog.Data.Models;

namespace WR.Blog.Business.Services
{
    public interface IBaseService : IDisposable
    {
        #region User Profile Methods
        /// <summary>
        /// Gets the user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>Returns the user profile or null if not found.</returns>
        UserProfile GetUser(string username);
        #endregion
    }
}
