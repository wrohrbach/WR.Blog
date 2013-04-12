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
    public class BaseServiceTest
    {
        private Mock<IBlogRepository> mockBlogRepository;
        private IQueryable<UserProfile> userProfiles;
        private BaseService service;

        #region Setup Methods
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            userProfiles = new List<UserProfile>{
                new UserProfile {
                    UserId = 1,
                    UserName = "user1",
                    DisplayName = "User 1"                    
                },
                new UserProfile {
                    UserId = 2,
                    UserName = "user2",
                    DisplayName = "User 2"                    
                },
                new UserProfile {
                    UserId = 3,
                    UserName = "user3",
                    DisplayName = "User 3"                    
                },
                new UserProfile {
                    UserId = 4,
                    UserName = "user4",
                    DisplayName = "User 4"                    
                }
            }.AsQueryable();
        }

        [SetUp]
        public void Setup()
        {
            mockBlogRepository = new Mock<IBlogRepository>();

            service = new SiteManagerService(mockBlogRepository.Object);
        }
        #endregion

        #region BaseService() Tests
        [Test]
        public void BaseService_should_create_an_instance_of_BaseService()
        {
            var baseService = new BaseService(mockBlogRepository.Object);

            Assert.IsNotNull(baseService);
            Assert.IsInstanceOf(typeof(BaseService), baseService);
        }
        #endregion

        #region GetUser(string username) Tests
        [Test]
        public void GetUser_should_return_an_object_of_type_UserProfile()
        {
            string username = "user1";

            mockBlogRepository.Setup(br => br.GetUserByUsername(username))
                .Returns(new UserProfile());

            var result = service.GetUser(username);

            Assert.IsInstanceOf(typeof(UserProfile), result);
        }

        [Test]
        public void GetUser_should_return_the_matched_user()
        {
            string username = "user1";

            mockBlogRepository.Setup(br => br.GetUserByUsername(username))
                .Returns(userProfiles.Where(u => u.UserName == username).SingleOrDefault());

            var result = service.GetUser(username);

            Assert.AreEqual(username, result.UserName);
        }

        [Test]
        public void GetUser_should_return_null_if_the_user_is_not_found()
        {
            string username = "User Does Not Exist";

            mockBlogRepository.Setup(br => br.GetUserByUsername(username))
                .Returns(userProfiles.Where(u => u.UserName == username).SingleOrDefault());

            var result = service.GetUser(username);

            Assert.IsNull(result);
        }

        [Test]
        public void GetUser_should_call_BlogRepository_GetUserByUsername()
        {
            string username = "user1";

            mockBlogRepository.Setup(br => br.GetUserByUsername(username))
                .Returns((UserProfile)null);

            var result = service.GetUser(username);

            mockBlogRepository.Verify((br => br.GetUserByUsername(username)), Times.Exactly(1));
        }
        #endregion
    }
}
