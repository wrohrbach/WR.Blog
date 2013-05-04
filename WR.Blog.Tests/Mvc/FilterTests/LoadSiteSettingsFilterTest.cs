using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using Moq;
using NUnit.Framework;

using WR.Blog.Business.Services;
using WR.Blog.Data.Models;
using WR.Blog.Data.Repositories;
using WR.Blog.Mvc.Filters;
using System.Web;
using System.Security.Principal;
using WR.Blog.Tests.Mvc.Fakes;
using System.Web.Routing;
using WR.Blog.Mvc.Areas.SiteAdmin.Models;

namespace WR.Blog.Tests.Mvc.FilterTests
{
    [TestFixture]
    public class LoadSiteSettingsFilterTest
    {
        private Mock<ISiteManagerService> mockSiteManagerService;
        private IQueryable<BlogSettingsDto> blogSettings;

        #region Setup Methods
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            blogSettings = new List<BlogSettingsDto>{
                new BlogSettingsDto {
                    Id = 1,
                    SiteTitle = "My Site Title",
                    Tagline = "Tagline",
                    AltTagline = "Alt Tagline",
                    MenuLinks = "About, Contact",
                    GoogleAccount = "2304782-1",
                    AllowComments = false,
                    PostsPerPage = 5,
                    LastModifiedDate = new DateTime(2013, 4, 9, 12, 12, 12)
                }
            }.AsQueryable();
        }

        [SetUp]
        public void Setup()
        {
            mockSiteManagerService = new Mock<ISiteManagerService>();
        }

        /// <summary>
        /// Gets the filter context for testing OnActionExecuting of LoadSiteSettingsFilter.
        /// </summary>
        /// <param name="isAdmin">if set to <c>true</c> [is admin].</param>
        /// <returns></returns>
        private ActionExecutingContext GetFilterContext(bool isAdmin)
        {
            var httpContext = new Mock<HttpContextBase>();
            var user = new Mock<IPrincipal>();

            user.Setup(u => u.IsInRole("Administrator")).Returns(isAdmin);
            httpContext.Setup(hc => hc.User).Returns(user.Object);

            var actionDescriptor = new Mock<ActionDescriptor>().Object;
            var controller = new FakeController();
            var controllerContext = new ControllerContext(httpContext.Object, new RouteData(), controller);
            
            return new ActionExecutingContext(controllerContext, actionDescriptor, new RouteValueDictionary());
        }
        #endregion

        #region LoadSiteSettingsFilter() Tests
        [Test]
        public void LoadSiteSettingsFilter_should_create_an_instance_of_LoadSiteSettingsFilter()
        {
            var attribute = new LoadSiteSettingsFilter(mockSiteManagerService.Object);

            Assert.IsNotNull(attribute);
            Assert.IsInstanceOf(typeof(LoadSiteSettingsFilter), attribute);
        }
        #endregion

        #region OnActionExecuting() Tests
        [Test]
        public void OnActionExecuting_ViewBag_BlogSettings_should_not_contain_null_value()
        {
            bool isAdmin = true;
            var attribute = new LoadSiteSettingsFilter(mockSiteManagerService.Object);
            var filterContext = GetFilterContext(isAdmin);
            mockSiteManagerService.Setup(br => br.GetBlogSettings()).Returns(blogSettings.FirstOrDefault());

            attribute.OnActionExecuting(filterContext);
            var viewBag = filterContext.Controller.ViewBag;

            Assert.IsNotNull(viewBag.BlogSettings);
        }

        [Test]
        public void OnActionExecuting_ViewBag_BlogSettings_should_be_an_object_of_type_BlogSettings()
        {
            bool isAdmin = true;
            var attribute = new LoadSiteSettingsFilter(mockSiteManagerService.Object);
            var filterContext = GetFilterContext(isAdmin);
            mockSiteManagerService.Setup(br => br.GetBlogSettings()).Returns(blogSettings.FirstOrDefault());

            attribute.OnActionExecuting(filterContext);
            var viewBag = filterContext.Controller.ViewBag;

            Assert.IsInstanceOf(typeof(BlogSettings), viewBag.BlogSettings);
        }

        [Test]
        public void OnActionExecuting_ViewBag_should_contain_default_site_settings_values()
        {
            bool isAdmin = true;
            var attribute = new LoadSiteSettingsFilter(mockSiteManagerService.Object);
            var filterContext = GetFilterContext(isAdmin);
            mockSiteManagerService.Setup(br => br.GetBlogSettings()).Returns((BlogSettingsDto)null);

            attribute.OnActionExecuting(filterContext);
            var result = (BlogSettings) filterContext.Controller.ViewBag.BlogSettings;

            Assert.AreEqual("Site Title", result.SiteTitle);
            Assert.AreEqual("Your blog's tagline.", result.Tagline);
            Assert.AreEqual("Alternate Tagline", result.AltTagline);
            Assert.AreEqual("", result.MenuLinks);
            Assert.AreEqual(10, result.PostsPerPage);
            Assert.AreEqual(true, result.AllowComments);
            Assert.AreEqual("", result.GoogleAccount);
        }

        [Test]
        public void OnActionExecuting_ViewBag_should_NOT_contain_default_site_settings_values()
        {
            bool isAdmin = true;
            var attribute = new LoadSiteSettingsFilter(mockSiteManagerService.Object);
            var filterContext = GetFilterContext(isAdmin);
            mockSiteManagerService.Setup(br => br.GetBlogSettings()).Returns(blogSettings.FirstOrDefault());

            attribute.OnActionExecuting(filterContext);
            var result = (BlogSettings) filterContext.Controller.ViewBag.BlogSettings;

            Assert.AreEqual("My Site Title", result.SiteTitle);
            Assert.AreEqual("Tagline", result.Tagline);
            Assert.AreEqual("Alt Tagline", result.AltTagline);
            Assert.AreEqual("About, Contact", result.MenuLinks);
            Assert.AreEqual(5, result.PostsPerPage);
            Assert.AreEqual(false, result.AllowComments);
            Assert.AreEqual("2304782-1", result.GoogleAccount);
        }

        [Test]
        public void OnActionExecuting_ViewBag_should_contain_true_for_ViewBagIsAdmin()
        {
            bool isAdmin = true;
            var attribute = new LoadSiteSettingsFilter(mockSiteManagerService.Object);
            var filterContext = GetFilterContext(isAdmin);
            mockSiteManagerService.Setup(br => br.GetBlogSettings()).Returns((BlogSettingsDto)null);

            attribute.OnActionExecuting(filterContext);
            var viewBag = filterContext.Controller.ViewBag;

            Assert.IsTrue(viewBag.BlogSettings.IsAdmin, "User is not admin");
        }

        [Test]
        public void OnActionExecuting_ViewBag_should_contain_false_for_ViewBagIsAdmin()
        {
            bool isAdmin = false;
            var attribute = new LoadSiteSettingsFilter(mockSiteManagerService.Object);
            var filterContext = GetFilterContext(isAdmin);
            mockSiteManagerService.Setup(br => br.GetBlogSettings()).Returns((BlogSettingsDto)null);

            attribute.OnActionExecuting(filterContext);
            var viewBag = filterContext.Controller.ViewBag;

            Assert.IsFalse(viewBag.BlogSettings.IsAdmin, "User is admin");
        }

        [Test]
        public void OnActionExecuting_ViewBag_should_contain_gravatar_url()
        {
            bool isAdmin = false;
            var attribute = new LoadSiteSettingsFilter(mockSiteManagerService.Object);
            var filterContext = GetFilterContext(isAdmin);
            mockSiteManagerService.Setup(br => br.GetBlogSettings()).Returns((BlogSettingsDto)null);

            attribute.OnActionExecuting(filterContext);
            var viewBag = filterContext.Controller.ViewBag;

            Assert.IsTrue(((string)viewBag.BlogSettings.GravatarUrl).StartsWith("http://gravatar.com/avatar/"), "Does not contain gravatar url");
        }
        #endregion
    }
}
