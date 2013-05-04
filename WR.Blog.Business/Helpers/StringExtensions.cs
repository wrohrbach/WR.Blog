using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WR.Blog.Business.Helpers
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns an MD5 hash of an email address for Gravatar images.
        /// </summary>
        /// <returns>MD5 hash of the email address.</returns>
        public static string GravatarHash(this string s)
        {
            using (var md5Hash = System.Security.Cryptography.MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(s.Trim().ToLower()));

                StringBuilder sb = new StringBuilder();
 
                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("x2"));
                }

                return sb.ToString(); 
            }
        }
    }
}