using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WR.Blog.Mvc.Helpers
{
    public static class IconLinkHelper
    {
        /// <summary>
        /// Builds an action link with an icon instead of only text.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="fragment">The url fragment (http://example.com#fragment).</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="iconHtmlAttributes">The icon HTML attributes.</param>
        /// <returns>
        /// Action link
        /// </returns>
        public static MvcHtmlString IconLink(this HtmlHelper htmlHelper, string actionName, string controllerName, string fragment, object routeValues, object htmlAttributes, object iconHtmlAttributes)
        {
            return IconLink(htmlHelper, String.Empty, actionName, controllerName, fragment, routeValues, htmlAttributes, iconHtmlAttributes);
        }

        /// <summary>
        /// Builds an action link with an icon instead of only text.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="iconHtmlAttributes">The img HTML attributes.</param>
        /// <returns>
        /// Action link
        /// </returns>
        public static MvcHtmlString IconLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string fragment, object routeValues, object htmlAttributes, object iconHtmlAttributes)
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            TagBuilder spanTag = new TagBuilder("span");
            spanTag.MergeAttributes(MakeDictionary(iconHtmlAttributes), true);
            string url = urlHelper.Action(actionName, controllerName, routeValues) + (string.IsNullOrEmpty(fragment) ? "" : "#" + fragment);

            TagBuilder imglink = new TagBuilder("a");
            imglink.MergeAttribute("href", url);
            imglink.InnerHtml = spanTag.ToString() + linkText;
            imglink.MergeAttributes(MakeDictionary(htmlAttributes), true);

            return new MvcHtmlString(imglink.ToString());
        }

        /// <summary>
        /// Makes a dictionary object out of anonymous type.
        /// </summary>
        /// <param name="withProperties">The with properties.</param>
        /// <returns>Dictionary Object</returns>
        private static IDictionary<string, object> MakeDictionary(object withProperties)
        {
            IDictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var properties = System.ComponentModel.TypeDescriptor.GetProperties(withProperties);
                foreach (System.ComponentModel.PropertyDescriptor property in properties)
                {
                    dict.Add(property.Name.Replace('_', '-'), property.GetValue(withProperties));
                }
            }
            catch
            {
                // Do nothing
            }

            return dict;
        }
    }
}