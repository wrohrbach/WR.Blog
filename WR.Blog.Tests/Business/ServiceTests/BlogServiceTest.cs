using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AutoMapper;
using Moq;
using NUnit.Framework;

using WR.Blog.Business.Services;
using WR.Blog.Data.Models;
using WR.Blog.Data.Repositories;
using WR.Blog.Mvc;

namespace WR.Blog.Tests.Business.ServiceTests
{
    [TestFixture]
    public class BlogServiceTest
    {
        private Mock<IBlogRepository> mockBlogRepository;
        private IQueryable<BlogPostDto> blogPosts;
        private IQueryable<BlogVersionDto> blogPostVersions;
        private BlogService blogger;

        #region Setup Methods
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            AutoMapperConfig.CreateMaps();
        }

        public void TestSetUp()
        {
            blogPosts = new List<BlogPostDto>{
                new BlogPostDto {
                    Id = 1,
                    Title = "Title 1",
                    UrlSegment = "title-1",
                    PublishedDate = new DateTime(2013, 3, 1),
                    IsPublished = true,
                    IsContentPage = true
                },
                new BlogPostDto {
                    Id = 2,
                    Title = "Title 2",
                    PublishedDate = new DateTime(2013, 2, 1),
                    IsPublished = true,
                    IsContentPage = false
                },
                new BlogPostDto {
                    Id = 3,
                    Title = "Title 3",
                    UrlSegment = "title-3",
                    PublishedDate = new DateTime(2013, 3, 3),
                    IsPublished = true,
                    IsContentPage = false
                },
                new BlogPostDto {
                    Id = 4,
                    Title = "Title 4",
                    UrlSegment = "title-4",
                    PublishedDate = new DateTime(2013, 3, 5),
                    IsPublished = false,
                    IsContentPage = false
                },
                new BlogPostDto {
                    Id = 5,
                    Title = "Title 5",
                    PublishedDate = new DateTime(2013, 3, 17),
                    IsPublished = true,
                    IsContentPage = false
                },
                new BlogPostDto {
                    Id = 6,
                    Title = "Title 6",
                    PublishedDate = new DateTime(2012, 3, 17),
                    IsPublished = true,
                    IsContentPage = false
                },
                new BlogPostDto {
                    Id = 7,
                    Title = "Title 7",
                    UrlSegment = "title-7",
                    PublishedDate = new DateTime(2013, 3, 3, 23, 59, 58),
                    IsPublished = true,
                    IsContentPage = false
                }
            }.AsQueryable();

            blogPostVersions = new List<BlogVersionDto>{
                new BlogVersionDto {
                    Id = 1,
                    Title = "Title 1",
                    UrlSegment = "title-1",
                    VersionOf = blogPosts.ToList()[0],
                    LastModifiedDate = new DateTime(2013, 3, 4)
                },
                new BlogVersionDto {
                    Id = 2,
                    Title = "Title 2",
                    VersionOf = blogPosts.ToList()[0],
                    LastModifiedDate = new DateTime(2013, 3, 3, 23, 59, 58)
                },
                new BlogVersionDto {
                    Id = 2,
                    Title = "Title 3",
                    VersionOf = blogPosts.ToList()[0],
                    LastModifiedDate = new DateTime(2013, 3, 4, 23, 59, 58)
                },
                new BlogVersionDto {
                    Id = 2,
                    Title = "Title 2",
                    VersionOf = blogPosts.ToList()[1]
                }
            }.AsQueryable();
        }

        [SetUp]
        public void Setup()
        {
            TestSetUp();

            mockBlogRepository = new Mock<IBlogRepository>();

            blogger = new BlogService(mockBlogRepository.Object);
        } 
        #endregion

        #region BlogService() Tests
        [Test]
        public void BlogService_should_create_an_instance_of_BlogService()
        {
            var blogService = new BlogService(mockBlogRepository.Object);

            Assert.IsNotNull(blogService);
            Assert.IsInstanceOf(typeof(BlogService), blogService);
        }
        #endregion

        #region GetBlogPostsByDate() Tests
        [Test]
        public void GetBlogPostsByDate_should_return_an_object_type_of_IEnumerable_BlogPost()
        {
            int? year = 2010;
            int? month = 1;
            int? day = 5;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostsByDate(year, month, day);

            Assert.IsInstanceOf(typeof(IEnumerable<BlogPostDto>), result);
        }

        [Test]
        public void GetBlogPostsByDate_should_return_blog_posts_published_on_specific_day()
        {
            int year = 2013;
            int month = 3;
            int day = 5;
            DateTime date = new DateTime(2013, month, day);

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostsByDate(year, month, day);

            Assert.AreNotEqual(null, result);
            foreach (var blogPost in result)
            {
                Assert.AreEqual(date.Date, blogPost.PublishedDate.Date);
            }
        }

        [Test]
        public void GetBlogPostsByDate_should_return_blog_posts_published_during_specific_month()
        {
            int year = 2013;
            int month = 3;
            int? day = null;
            DateTime dateFrom = new DateTime(year, month, 1);
            DateTime dateTo = dateFrom.AddMonths(1).AddSeconds(-1);

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostsByDate(year, month, day);

            Assert.AreNotEqual(null, result);
            foreach (var blogPost in result)
            {
                Assert.LessOrEqual(dateFrom, blogPost.PublishedDate);
                Assert.GreaterOrEqual(dateTo, blogPost.PublishedDate);
            }
        }

        [Test]
        public void GetBlogPostsByDate_should_return_blog_posts_published_during_specific_year()
        {
            int year = 2013;
            int? month = null;
            int? day = null;
            DateTime dateFrom = new DateTime(year, 1, 1);
            DateTime dateTo = new DateTime(year + 1, 1, 1).AddSeconds(-1);

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostsByDate(year, month, day);

            Assert.AreNotEqual(null, result);
            foreach (var blogPost in result)
            {
                Assert.LessOrEqual(dateFrom, blogPost.PublishedDate);
                Assert.GreaterOrEqual(dateTo, blogPost.PublishedDate);
            }
        }

        [Test]
        public void GetBlogPostsByDate_should_return_zero_blog_posts_for_specific_date()
        {
            int year = 2013;
            int? month = 1;
            int? day = 1;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostsByDate(year, month, day);

            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void GetBlogPostsByDate_should_return_zero_blog_posts_for_specific_month()
        {
            int year = 2013;
            int? month = 1;
            int? day = null;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostsByDate(year, month, day);

            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void GetBlogPostsByDate_should_return_zero_blog_posts_for_specific_year()
        {
            int year = 2010;
            int? month = null;
            int? day = null;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostsByDate(year, month, day);

            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void GetBlogPostsByDate_should_return_only_blog_posts_where_IsPublished_is_true()
        {
            int year = 2013;
            int? month = null;
            int? day = null;
            bool isPublished = true;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostsByDate(year, month, day, isPublished);

            bool onlyPublished = true;
            foreach (var blogPost in result)
            {
                if (!blogPost.IsPublished)
                {
                    onlyPublished = false;
                }
            }
            Assert.IsTrue(onlyPublished);
        }

        [Test]
        public void GetBlogPostsByDate_should_return_only_blog_posts_whether_IsPublished_is_true_or_false()
        {
            int year = 2013;
            int? month = null;
            int? day = null;
            bool isPublished = false;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostsByDate(year, month, day, isPublished);

            bool onlyPublished = true;
            bool onlyUnpublished = true;
            foreach (var blogPost in result)
            {
                if (!blogPost.IsPublished)
                {
                    onlyPublished = false;
                }
                else
                {
                    onlyUnpublished = false;
                }
            }
            Assert.IsFalse(onlyPublished);
            Assert.IsFalse(onlyUnpublished);
        }

        [Test]
        public void GetBlogPostsByDate_should_return_only_blog_posts_where_IsContentPage_is_false()
        {
            int year = 2013;
            int? month = null;
            int? day = null;
            bool isPublished = false;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostsByDate(year, month, day, isPublished);

            if (blogPosts.Where(bp => bp.IsContentPage && bp.PublishedDate.Year == 2013).FirstOrDefault() == null)
            {
                throw new Exception("No Content Pages to test against.");
            }

            bool hasContentPages = false;
            foreach (var blogPost in result)
            {
                if (blogPost.IsContentPage)
                {
                    hasContentPages = true;
                }
            }
            Assert.IsFalse(hasContentPages);
        }

        [Test]
        public void GetBlogPostsByDate_should_return_null_based_on_year_is_null()
        {
            int? year = null;
            int? month = 3;
            int? day = 1;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostsByDate(year, month, day);

            Assert.AreEqual(null, result);
        }

        [Test]
        public void GetBlogPostsByDate_should_return_null_based_on_year_month_day_are_null()
        {
            int? year = null;
            int? month = null;
            int? day = null;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostsByDate(year, month, day);

            Assert.AreEqual(null, result);
        }

        [Test]
        public void GetBlogPostsByDate_should_not_call_BlogRepository_GetBlogPosts_based_on_year_is_null()
        {
            int? year = null;
            int? month = 3;
            int? day = 1;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostsByDate(year, month, day);

            mockBlogRepository.Verify((br => br.GetBlogPosts()), Times.Never());
        }

        [Test]
        public void GetBlogPostsByDate_should_not_call_BlogRepository_GetBlogPosts_based_on_year_month_day_are_null()
        {
            int? year = null;
            int? month = null;
            int? day = null;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostsByDate(year, month, day);

            mockBlogRepository.Verify((br => br.GetBlogPosts()), Times.Never());
        }

        [Test]
        public void GetBlogPostsByDate_should_not_call_BlogRepository_GetBlogPosts_based_on_invalid_year()
        {
            int? year = 10001;
            int? month = 1;
            int? day = 1;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostsByDate(year, month, day);

            mockBlogRepository.Verify((br => br.GetBlogPosts()), Times.Never());
        }

        [Test]
        public void GetBlogPostsByDate_should_not_call_BlogRepository_GetBlogPosts_based_on_invalid_month()
        {
            int? year = 2010;
            int? month = 13;
            int? day = 1;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostsByDate(year, month, day);

            mockBlogRepository.Verify((br => br.GetBlogPosts()), Times.Never());
        }

        [Test]
        public void GetBlogPostsByDate_should_not_call_BlogRepository_GetBlogPosts_based_on_invalid_day()
        {
            int? year = 2010;
            int? month = 2;
            int? day = 29;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostsByDate(year, month, day);

            mockBlogRepository.Verify((br => br.GetBlogPosts()), Times.Never());
        }

        [Test]
        public void GetBlogPostsByDate_should_call_BlogRepository_GetBlogPosts_based_on_year_month_day_are_not_null()
        {
            int? year = 2010;
            int? month = 1;
            int? day = 5;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostsByDate(year, month, day);

            mockBlogRepository.Verify((br => br.GetBlogPosts()), Times.Exactly(1));
        }
        #endregion

        #region GetBlogPost(int id) Tests
        [Test]
        public void GetBlogPostInt_should_return_an_object_of_type_BlogPost()
        {
            int id = 5;

            mockBlogRepository.Setup(br => br.GetBlogPostById(id)).Returns(blogPosts.Where(b => b.Id == id).SingleOrDefault());

            var result = blogger.GetBlogPost(id);

            Assert.IsInstanceOf(typeof(BlogPostDto), result);
        }

        [Test]
        public void GetBlogPostInt_should_return_the_correct_BlogPost()
        {
            int id = 5;

            mockBlogRepository.Setup(br => br.GetBlogPostById(id)).Returns(blogPosts.Where(b => b.Id == id).SingleOrDefault());

            var result = blogger.GetBlogPost(id);

            Assert.AreEqual(id, result.Id);
        }
        #endregion

        #region GetBlogPosts(int? skip, int? take, bool published, bool content) Tests
        [Test]
        public void GetBlogPostsIntIntBoolBoolBool_should_return_an_object_of_type_IEnumerable_BlogPost()
        {
            int? skip = null;
            int? take = null;
            bool published = false;
            bool content = false;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPosts(skip, take, published, content);

            Assert.IsInstanceOf(typeof(IEnumerable<BlogPostDto>), result);
        }

        [Test]
        public void GetBlogPostsIntIntBoolBoolBool_should_return_two_BlogPosts()
        {
            int? skip = null;
            int? take = 2;
            bool published = false;
            bool content = false;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPosts(skip, take, published, content);

            Assert.AreEqual(take, result.Count());
        }

        [Test]
        public void GetBlogPostsIntIntBoolBoolBool_should_not_skip_without_a_take_value()
        {
            int? skip = 2;
            int? take = null;
            bool published = false;
            bool content = true;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPosts(skip, take, published, content);

            Assert.AreEqual(blogPosts.Count(), result.Count());
        }

        [Test]
        public void GetBlogPostsIntIntBoolBoolBool_should_skip_4_and_take_2()
        {
            int? skip = 3; // skipValue = (skip - 1) * take;
            int? take = 2;
            bool published = false;
            bool content = true;
            bool orderDescending = false;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPosts(skip, take, published, content, orderDescending);
            var expected = blogPosts.Skip(4).Take(2).ToList();

            int counter = 0;
            foreach (var post in result)
            {
                Assert.AreEqual(expected[counter++].Id, post.Id);
            }
        }

        [Test]
        public void GetBlogPostsIntIntBoolBoolBool_should_return_published_and_unpublished_posts()
        {
            int? skip = null; // skipValue = (skip - 1) * take;
            int? take = null;
            bool published = false;
            bool content = true;
            bool orderDescending = false;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPosts(skip, take, published, content, orderDescending);

            bool unpublishedFound = false;
            foreach (var post in result)
            {
                if (!post.IsPublished)
                {
                    unpublishedFound = true;
                }
            }
            Assert.IsTrue(unpublishedFound);
        }

        [Test]
        public void GetBlogPostsIntIntBoolBoolBool_should_return_only_published_posts()
        {
            int? skip = null; // skipValue = (skip - 1) * take;
            int? take = null;
            bool published = true;
            bool content = true;
            bool orderDescending = false;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPosts(skip, take, published, content, orderDescending);

            foreach (var post in result)
            {
                Assert.IsTrue(post.IsPublished);
            }
        }

        [Test]
        public void GetBlogPostsIntIntBoolBoolBool_should_include_content_pages()
        {
            int? skip = null; // skipValue = (skip - 1) * take;
            int? take = null;
            bool published = true;
            bool content = true;
            bool orderDescending = false;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPosts(skip, take, published, content, orderDescending);

            bool contentPageFound = false;
            foreach (var page in result)
            {
                if (page.IsContentPage)
                {
                    contentPageFound = true;
                }
            }
            Assert.IsTrue(contentPageFound);
        }

        [Test]
        public void GetBlogPostsIntIntBoolBoolBool_should_not_include_content_pages()
        {
            int? skip = null; // skipValue = (skip - 1) * take;
            int? take = null;
            bool published = true;
            bool content = false;
            bool orderDescending = false;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPosts(skip, take, published, content, orderDescending);

            bool contentPageFound = false;
            foreach (var page in result)
            {
                if (page.IsContentPage)
                {
                    contentPageFound = true;
                }
            }
            Assert.IsFalse(contentPageFound);
        }

        [Test]
        public void GetBlogPostsIntIntBoolBoolBool_should_order_by_descending_published_date()
        {
            int? skip = null; // skipValue = (skip - 1) * take;
            int? take = null;
            bool published = false;
            bool content = true;
            bool orderDescending = true;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPosts(skip, take, published, content, orderDescending);

            bool isOrderedDescending = true;
            DateTime laterDate = DateTime.MaxValue;
            foreach (var post in result)
            {
                if (post.PublishedDate > laterDate)
                {
                    isOrderedDescending = false;
                }
                laterDate = post.PublishedDate;
            }
            Assert.IsTrue(isOrderedDescending);
        }

        [Test]
        public void GetBlogPostsIntIntBoolBoolBool_should_not_order_by_descending_published_date()
        {
            int? skip = null; // skipValue = (skip - 1) * take;
            int? take = null;
            bool published = false;
            bool content = true;
            bool orderDescending = false;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPosts(skip, take, published, content, orderDescending);

            bool isOrderedDescending = true;
            DateTime laterDate = DateTime.MaxValue;
            foreach (var post in result)
            {
                if (post.PublishedDate > laterDate)
                {
                    isOrderedDescending = false;
                }
                laterDate = post.PublishedDate;
            }
            Assert.IsFalse(isOrderedDescending);
        }

        [Test]
        public void GetBlogPostsIntIntBoolBoolBool_should_return_all_blog_posts()
        {
            int? skip = null; // skipValue = (skip - 1) * take;
            int? take = null;
            bool published = false;
            bool content = true;
            bool orderDescending = false;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPosts(skip, take, published, content, orderDescending);

            Assert.AreEqual(blogPosts.Count(), result.Count());
        }

        [Test]
        public void GetBlogPostsIntIntBoolBoolBool_should_call_BlogRepository_GetBlogPosts()
        {
            int? skip = null;
            int? take = null;
            bool published = false;
            bool content = false;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPosts(skip, take, published, content);

            mockBlogRepository.Verify((br => br.GetBlogPosts()), Times.Exactly(1));
        }
        #endregion

        #region GetBlogPostByUrlSegment(string urlSegment, bool isContentPage = false) Tests
        [Test]
        public void GetBlogPostByUrlSegmentStringBool_should_return_an_object_of_type_BlogPost()
        {
            string urlSegment = "title-1";
            bool isContentPage = true;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostByUrlSegment(urlSegment, isContentPage);

            Assert.IsInstanceOf(typeof(BlogPostDto), result);
        }

        [Test]
        public void GetBlogPostByUrlSegmentStringBool_should_return_null_no_url_segment_match()
        {
            string urlSegment = "not a url segment";
            bool isContentPage = true;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostByUrlSegment(urlSegment, isContentPage);

            Assert.IsNull(result);
        }

        [Test]
        public void GetBlogPostByUrlSegmentStringBool_should_not_return_null_url_segment_match_but_not_content_page()
        {
            string urlSegment = "title-7";
            bool isContentPage = false;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostByUrlSegment(urlSegment, isContentPage);

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetBlogPostByUrlSegmentStringBool_should_return_a_matching_url_segment()
        {
            string urlSegment = "title-7";
            bool isContentPage = false;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostByUrlSegment(urlSegment, isContentPage);

            Assert.AreEqual(urlSegment, result.UrlSegment);
        }

        [Test]
        public void GetBlogPostByUrlSegmentStringBool_should_return_a_matching_url_segment_and_content_page()
        {
            string urlSegment = "title-1";
            bool isContentPage = true;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostByUrlSegment(urlSegment, isContentPage);

            Assert.AreEqual(urlSegment, result.UrlSegment);
            Assert.IsTrue(result.IsContentPage);
        }

        [Test]
        public void GetBlogPostByUrlSegmentStringBool_should_return_a_matching_url_segment_even_though_not_specified_as_content_page()
        {
            string urlSegment = "title-1";
            bool isContentPage = false;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostByUrlSegment(urlSegment, isContentPage);

            Assert.AreEqual(urlSegment, result.UrlSegment);
        }

        [Test]
        public void GetBlogPostByUrlSegmentStringBool_should_call_BlogRepository_GetBlogPosts()
        {
            string urlSegment = "title-1";
            bool isContentPage = false;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostByUrlSegment(urlSegment, isContentPage);

            mockBlogRepository.Verify((br => br.GetBlogPosts()), Times.Exactly(1));
        }
        #endregion

        #region GetBlogPostByPermalink(int? year, int? month, int? day, string urlSegment, bool isPublished = true) Tests
        [Test]
        public void GetBlogPostByPermalink_should_return_an_object_of_type_BlogPost()
        {
            int year = 2013;
            int month = 3;
            int day = 3;
            string urlSegment = "title-3";
            bool isPublished = true;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostByPermalink(year, month, day, urlSegment, isPublished);

            Assert.IsInstanceOf(typeof(BlogPostDto), result);
        }

        [Test]
        public void GetBlogPostByPermalink_should_return_null_because_attempted_match_is_content_page()
        {
            int year = 2013;
            int month = 3;
            int day = 1;
            string urlSegment = "title-1";
            bool isPublished = true;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostByPermalink(year, month, day, urlSegment, isPublished);

            Assert.IsNull(result);
        }

        [Test]
        public void GetBlogPostByPermalink_should_return_null_because_no_UrlSegment_match()
        {
            int year = 2013;
            int month = 3;
            int day = 3;
            string urlSegment = "Not a url segment";
            bool isPublished = true;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostByPermalink(year, month, day, urlSegment, isPublished);

            Assert.IsNull(result);
        }

        [Test]
        public void GetBlogPostByPermalink_should_return_null_because_matched_UrlSegment_has_IsPublished_flag_set_to_false()
        {
            int year = 2013;
            int month = 3;
            int day = 5;
            string urlSegment = "title-4";
            bool isPublished = true;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostByPermalink(year, month, day, urlSegment, isPublished);

            Assert.IsNull(result);
        }

        [Test]
        public void GetBlogPostByPermalink_should_match_url_segment_and_IsPublished_flag_is_true()
        {
            int year = 2013;
            int month = 3;
            int day = 3;
            string urlSegment = "title-3";
            bool isPublished = true;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostByPermalink(year, month, day, urlSegment, isPublished);

            Assert.IsNotNull(result);
            Assert.AreEqual(urlSegment, result.UrlSegment);
        }

        [Test]
        public void GetBlogPostByPermalink_should_match_url_segment_and_IsPublished_flag_is_false()
        {
            int year = 2013;
            int month = 3;
            int day = 5;
            string urlSegment = "title-4";
            bool isPublished = false;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostByPermalink(year, month, day, urlSegment, isPublished);

            Assert.IsNotNull(result);
            Assert.AreEqual(urlSegment, result.UrlSegment);
        }

        [Test]
        public void GetBlogPostByPermalink_should_call_BlogRepository_GetBlogPosts()
        {
            int year = 2013;
            int month = 3;
            int day = 5;
            string urlSegment = "title-4";
            bool isPublished = false;

            mockBlogRepository.Setup(br => br.GetBlogPosts()).Returns(blogPosts);

            var result = blogger.GetBlogPostByPermalink(year, month, day, urlSegment, isPublished);

            mockBlogRepository.Verify((br => br.GetBlogPosts()), Times.Exactly(1));
        }
        #endregion

        #region AddBlogPost(BlogPost blogPost) Tests
        [Test]
        public void AddBlogPost_should_call_BlogRepository_AddBlogPost()
        {
            BlogPostDto blogPost = new BlogPostDto();

            blogger.AddBlogPost(blogPost);

            mockBlogRepository.Verify((br => br.AddBlogPost(blogPost)), Times.Exactly(1));
        }
        #endregion

        #region UpdateBlogPost(BlogPost blogPost) Tests
        [Test]
        public void UpdateBlogPost_should_call_BlogRepository_UpdateBlogPost()
        {
            BlogPostDto blogPost = new BlogPostDto();

            blogger.UpdateBlogPost(blogPost);

            mockBlogRepository.Verify((br => br.UpdateBlogPost(blogPost)), Times.Exactly(1));
        }
        #endregion

        #region DeleteBlogPost(int id) Tests
        [Test]
        public void DeleteBlogPost_should_call_BlogRepository_DeleteBlogPost()
        {
            int id = 0;

            blogger.DeleteBlogPost(id);

            mockBlogRepository.Verify((br => br.DeleteBlogPost(id)), Times.Exactly(1));
        }
        #endregion

        #region SaveBlogPostAsVersion(BlogPost blogPost) Tests
        [Test]
        public void SaveBlogPostAsVersion_should_return_an_int()
        {
            BlogPostDto blogPost = blogPosts.ToList()[0];
            int expectedResult = 1;

            mockBlogRepository.Setup(br => br.AddBlogPostVersion(Moq.It.IsAny<BlogVersionDto>())).Returns(expectedResult);

            var result = blogger.SaveBlogPostAsVersion(blogPost);

            Assert.IsInstanceOf<int>(result);
        }

        [Test]
        public void SaveBlogPostAsVersion_should_return_the_expected_int_value()
        {
            BlogPostDto blogPost = blogPosts.ToList()[0];
            int expectedResult = 3527;

            mockBlogRepository.Setup(br => br.AddBlogPostVersion(Moq.It.IsAny<BlogVersionDto>())).Returns(expectedResult);

            var result = blogger.SaveBlogPostAsVersion(blogPost);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void SaveBlogPostAsVersion_should_call_BlogRepository_AddBlogPostVersion()
        {
            BlogPostDto blogPost = blogPosts.ToList()[0];
            int expectedResult = 1;

            mockBlogRepository.Setup(br => br.AddBlogPostVersion(Moq.It.IsAny<BlogVersionDto>())).Returns(expectedResult);

            var result = blogger.SaveBlogPostAsVersion(blogPost);

            mockBlogRepository.Verify((br => br.AddBlogPostVersion(Moq.It.IsAny<BlogVersionDto>())), Times.Exactly(1));
        }
        #endregion

        #region SaveBlogPostAsVersion(int blogPostId)
        // TODO: Write tests for SaveBlogPostAsVersion(int blogPostId)
        #endregion

        #region PublishVersion(BlogPostVersion version) Tests
        [Test]
        public void PublishVersion_VersionOf_should_be_the_same_as_version()
        {
            BlogVersionDto version = blogPostVersions.ToList()[1];

            blogger.PublishVersion(version);
            var expected = version.VersionOf;

            Assert.AreEqual(expected.Title, version.Title);
            Assert.AreNotEqual(expected.Id, version.Id);
            Assert.IsTrue(expected.Id > 0);
        }

        [Test]
        public void PublishVersion_should_call_BlogRepository_UpdateBlogPostVersion()
        {
            BlogVersionDto version = blogPostVersions.ToList()[1];

            mockBlogRepository.Setup(br => br.UpdateBlogPostVersion(Moq.It.IsAny<BlogVersionDto>()));

            blogger.PublishVersion(version);

            mockBlogRepository.Verify((br => br.UpdateBlogPostVersion(Moq.It.IsAny<BlogVersionDto>())), Times.Exactly(1));
        }
        #endregion

        #region GetVersion(int id) Tests
        [Test]
        public void GetVersion_should_return_an_object_of_type_BlogPostVersion()
        {
            int versionId = 1;

            mockBlogRepository.Setup(br => br.GetBlogPostVersionById(versionId)).Returns(blogPostVersions.Where(v => v.Id == versionId).SingleOrDefault());

            var result = blogger.GetVersion(versionId);

            Assert.IsInstanceOf<BlogVersionDto>(result);
        }

        [Test]
        public void GetVersion_should_return_the_correct_BlogPostVersion()
        {
            int versionId = 1;

            mockBlogRepository.Setup(br => br.GetBlogPostVersionById(versionId)).Returns(blogPostVersions.Where(v => v.Id == versionId).SingleOrDefault());

            var result = blogger.GetVersion(versionId);

            Assert.AreEqual(versionId, result.Id);
        }

        [Test]
        public void GetVersion_should_return_the_null_when_a_version_is_not_found()
        {
            int versionId = 27;

            mockBlogRepository.Setup(br => br.GetBlogPostVersionById(versionId)).Returns(blogPostVersions.Where(v => v.Id == versionId).SingleOrDefault());

            var result = blogger.GetVersion(versionId);

            Assert.IsNull(result);
        }

        [Test]
        public void GetVersion_should_call_BlogRepository_GetVersionById_one_time()
        {
            int versionId = 27;

            mockBlogRepository.Setup(br => br.GetBlogPostVersionById(Moq.It.IsAny<int>()))
                .Returns((BlogVersionDto)null);

            var result = blogger.GetVersion(versionId);

            mockBlogRepository.Verify((br => br.GetBlogPostVersionById(Moq.It.IsAny<int>())), Times.Exactly(1));
        }
        #endregion

        #region GetVersionsByBlogPost(int blogPostId) Tests
        [Test]
        public void GetVersionsByBlogPostIntId_should_return_an_object_of_type_IEnumerable_BlogPostVersion()
        {
            int blogPostId = 1;

            mockBlogRepository.Setup(br => br.GetBlogPostVersions())
                .Returns(blogPostVersions);

            var result = blogger.GetVersionsByBlogPost(blogPostId);

            Assert.IsInstanceOf(typeof(IEnumerable<BlogVersionDto>), result);
        }

        [Test]
        public void GetVersionsByBlogPostIntId_should_return_only_versions_of_the_specified_blog_post()
        {
            int blogPostId = 1;

            mockBlogRepository.Setup(br => br.GetBlogPostVersions())
                .Returns(blogPostVersions);

            var result = blogger.GetVersionsByBlogPost(blogPostId);

            Assert.IsTrue(result.Count() > 0);

            foreach (var version in result)
            {
                Assert.AreEqual(blogPostId, version.VersionOf.Id);
            }
        }

        [Test]
        public void GetVersionsByBlogPostIntId_should_return_an_empty_collection_of_versions()
        {
            int blogPostId = 27;

            mockBlogRepository.Setup(br => br.GetBlogPostVersions())
                .Returns(blogPostVersions);

            var result = blogger.GetVersionsByBlogPost(blogPostId);

            Assert.IsTrue(result.Count() == 0);
        }

        [Test]
        public void GetVersionsByBlogPostIntId_should_return_versions_in_descending_order_by_date_modified()
        {
            int blogPostId = 1;

            mockBlogRepository.Setup(br => br.GetBlogPostVersions())
                .Returns(blogPostVersions);

            var result = blogger.GetVersionsByBlogPost(blogPostId);

            Assert.IsTrue(result.Count() > 0, "No results returned.");

            bool isOrderedDescending = true;
            DateTime laterDate = DateTime.MaxValue;
            foreach (var version in result)
            {
                if (version.LastModifiedDate > laterDate)
                {
                    isOrderedDescending = false;
                }
                laterDate = version.LastModifiedDate;
            }
            Assert.IsTrue(isOrderedDescending, "Results are not in descending order by date modified.");
        }

        [Test]
        public void GetVersionsByBlogPostIntId_should_call_BlogRepository_GetVersionsByBlogPost()
        {
            int blogPostId = 27;

            mockBlogRepository.Setup(br => br.GetBlogPostVersions())
                .Returns(blogPostVersions);

            var result = blogger.GetVersionsByBlogPost(blogPostId);

            mockBlogRepository.Verify((br => br.GetBlogPostVersions()), Times.Exactly(1));
        }
        #endregion

        #region GetVersionsByBlogPost(BlogPost blogPost) Tests
        [Test]
        public void GetVersionsByBlogPost_BlogPost_should_return_an_object_of_type_IEnumerable_BlogPostVersion()
        {
            BlogPostDto blogPost = blogPosts.ToList()[0];

            var result = blogger.GetVersionsByBlogPost(blogPost);

            Assert.IsInstanceOf(typeof(IEnumerable<BlogVersionDto>), result);
        }

        [Test]
        public void GetVersionsByBlogPost_BlogPost_should_return_only_versions_of_the_specified_blog_post()
        {
            BlogPostDto blogPost = blogPosts.ToList()[0];

            mockBlogRepository.Setup(br => br.GetBlogPostVersions())
                .Returns(blogPostVersions);

            var result = blogger.GetVersionsByBlogPost(blogPost);

            Assert.IsTrue(result.Count() > 0);

            foreach (var version in result)
            {
                Assert.AreEqual(blogPost.Id, version.VersionOf.Id);
            }
        }

        [Test]
        public void GetVersionsByBlogPost_BlogPost_should_return_an_empty_collection_of_versions()
        {
            int blogPostId = 27;

            mockBlogRepository.Setup(br => br.GetBlogPostVersions())
                .Returns(blogPostVersions);

            var result = blogger.GetVersionsByBlogPost(blogPostId);

            Assert.IsTrue(result.Count() == 0);
        }
        #endregion

        #region UpdateVersion(BlogPostVersion version) Tests
        [Test]
        public void UpdateVersion_should_call_BlogRepository_UpdateBlogPostVersion()
        {
            BlogVersionDto version = new BlogVersionDto();

            blogger.UpdateVersion(version);

            mockBlogRepository.Verify((br => br.UpdateBlogPostVersion(version)), Times.Exactly(1));
        }
        #endregion

        #region DeleteVersion(int id) Tests
        [Test]
        public void DeleteVersion_should_call_BlogRepository_DeleteBlogPostVersion()
        {
            int id = 0;

            blogger.DeleteVersion(id);

            mockBlogRepository.Verify((br => br.DeleteBlogPostVersion(id)), Times.Exactly(1));
        }
        #endregion
    }
}
