using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WR.Blog.Data.Models;
using WR.Blog.Data.Repositories;

namespace WR.Blog.Business.Services
{
    public class BaseService : IBaseService
    {
        protected readonly IBlogRepository br;

        public BaseService(IBlogRepository br)
        {
            this.br = br;
        }

        #region User Profile Methods
        public UserProfile GetUser(string username)
        {
            return br.GetUserByUsername(username);
        }
        #endregion

        public void Dispose()
        {
            br.Dispose();
        }
    }
}
