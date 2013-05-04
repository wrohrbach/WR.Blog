using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Moq;
using NUnit.Framework;

using WR.Blog.Business.Services;
using WR.Blog.Data.Models;
using WR.Blog.Data.Repositories;

namespace WR.Blog.Tests.Business.ServiceTests
{
    [TestFixture]
    public class SiteManagerServiceTest
    {
        private Mock<IBlogRepository> mockBlogRepository;
        private IQueryable<BlogSettingsDto> blogSettings;
        private SiteManagerService manager;

        #region Setup Methods
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            blogSettings = new List<BlogSettingsDto>{
                new BlogSettingsDto {
                    Id = 1,
                    SiteTitle = "Site Title",
                    Tagline = "Tagline",
                    AltTagline = "Alternate Tagline",
                    MenuLinks = "About, Contact",
                    GoogleAccount = "2304782-1",
                    AllowComments = true,
                    PostsPerPage = 10,
                    LastModifiedDate = new DateTime(2013, 4, 9, 12, 12, 12)
                }
            }.AsQueryable();
        }

        [SetUp]
        public void Setup()
        {
            mockBlogRepository = new Mock<IBlogRepository>();

            manager = new SiteManagerService(mockBlogRepository.Object);
        }
        #endregion

        #region SiteManagerService() Tests
        [Test]
        public void SiteManagerService_should_create_an_instance_of_SiteManagerService()
        {
            var managerService = new SiteManagerService(mockBlogRepository.Object);

            Assert.IsNotNull(managerService);
            Assert.IsInstanceOf(typeof(SiteManagerService), managerService);
        }
        #endregion

        #region GetBlogSettings() Tests
        [Test]
        public void GetBlogSettingss_should_return_an_object_of_type_SiteSettings()
        {
            mockBlogRepository.Setup(br => br.GetBlogSettings()).Returns(blogSettings.FirstOrDefault());

            var result = manager.GetBlogSettings();

            Assert.IsInstanceOf(typeof(BlogSettingsDto), result);
        }

        [Test]
        public void GetBlogSettings_should_return_new_SiteSettings_object_if_BlogRepository_GetSiteSettings_returns_null()
        {
            mockBlogRepository.Setup(br => br.GetBlogSettings()).Returns((BlogSettingsDto)null);

            var result = manager.GetBlogSettings();

            Assert.AreEqual(0, result.Id);
        }

        [Test]
        public void GetBlogSettings_should_return_site_settings()
        {
            mockBlogRepository.Setup(br => br.GetBlogSettings()).Returns(blogSettings.FirstOrDefault());

            var result = manager.GetBlogSettings();

            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("2304782-1", result.GoogleAccount);
        }

        [Test]
        public void GetBlogSettings_should_call_BlogRepository_GetSiteSettings()
        {
            mockBlogRepository.Setup(br => br.GetBlogSettings()).Returns(blogSettings.FirstOrDefault());

            var result = manager.GetBlogSettings();

            mockBlogRepository.Verify((br => br.GetBlogSettings()), Times.Exactly(1));
        }
        #endregion

        #region AddOrUpdateBlogSettings(BlogSettings settings) Tests
        [Test]
        public void AddOrUpdateBlogSettings_should_call_BlogRepository_AddOrUpdateSiteSettings()
        {
            var settings = new BlogSettingsDto();

            manager.AddOrUpdateBlogSettings(settings);

            mockBlogRepository.Verify((br => br.AddOrUpdateBlogSettings(settings)), Times.Exactly(1));
        }
        #endregion
    }
}
