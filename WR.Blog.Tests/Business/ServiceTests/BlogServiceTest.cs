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
        private IQueryable<BlogCommentDto> blogComments;
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
                    Id = 3,
                    Title = "Title 3",
                    VersionOf = blogPosts.ToList()[0],
                    LastModifiedDate = new DateTime(2013, 3, 4, 23, 59, 58)
                },
                new BlogVersionDto {
                    Id = 4,
                    Title = "Title 2",
                    VersionOf = blogPosts.ToList()[1]
                }
            }.AsQueryable();

            blogComments = new List<BlogCommentDto>{
                new BlogCommentDto {
                    Id = 1,
                    BlogPost = blogPosts.ToList()[0],
                    Title = "Title 1",
                    CommentDate = new DateTime(2013, 3, 4),
                    IsApproved = true
                },
                new BlogCommentDto {
                    Id = 2,
                    BlogPost = blogPosts.ToList()[0],
                    Title = "Title 1",
                    CommentDate = new DateTime(2013, 3, 4, 23, 59, 58),
                    IsApproved = false
                },
                new BlogCommentDto {
                    Id = 3,
                    BlogPost = blogPosts.ToList()[0],
                    Title = "Title 1",
                    CommentDate = new DateTime(2013, 3, 3, 23, 59, 58),
                    IsApproved = true
                },
                new BlogCommentDto {
                    Id = 4,
                    BlogPost = blogPosts.ToList()[2],
                    Title = "Title 1",
                    CommentDate = new DateTime(2013, 3, 4),
                    IsApproved = true
                },
                new BlogCommentDto {
                    Id = 5,
                    BlogPost = blogPosts.ToList()[2],
                    Title = "Title 1",
                    CommentDate = new DateTime(2013, 3, 5),
                    IsApproved = true
                },
                new BlogCommentDto {
                    Id = 6,
                    BlogPost = blogPosts.ToList()[2],
                    Title = "Title 1",
                    CommentDate = new DateTime(2013, 3, 4),
                    IsApproved = false 
                },
                new BlogCommentDto {
                    Id = 7,
                    BlogPost = blogPosts.ToList()[2],
                    Title = "Title 1",
                    CommentDate = new DateTime(2013, 3, 5),
                    IsApproved = false 
                },
                new BlogCommentDto {
                    Id = 8,
                    BlogPost = blogPosts.ToList()[2],
                    Title = "Title 1",
                    CommentDate = new DateTime(2013, 3, 4),
                    IsApproved = false 
                },
                new BlogCommentDto {
                    Id = 9,
                    BlogPost = blogPosts.ToList()[2],
                    Title = "Title 1",
                    CommentDate = new DateTime(2013, 3, 5),
                    IsApproved = false 
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

            mockBlogRepository.Verify((br => br.GetBlogPosts()), Times.Once());
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

            mockBlogRepository.Verify((br => br.GetBlogPosts()), Times.Once());
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

            mockBlogRepository.Verify((br => br.GetBlogPosts()), Times.Once());
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

            mockBlogRepository.Verify((br => br.GetBlogPosts()), Times.Once());
        }
        #endregion

        #region AddBlogPost(BlogPost blogPost) Tests
        [Test]
        public void AddBlogPost_should_call_BlogRepository_AddBlogPost()
        {
            BlogPostDto blogPost = new BlogPostDto();

            blogger.AddBlogPost(blogPost);

            mockBlogRepository.Verify((br => br.AddBlogPost(blogPost)), Times.Once());
        }
        #endregion

        #region UpdateBlogPost(BlogPost blogPost) Tests
        [Test]
        public void UpdateBlogPost_should_call_BlogRepository_UpdateBlogPost()
        {
            BlogPostDto blogPost = new BlogPostDto();

            blogger.UpdateBlogPost(blogPost);

            mockBlogRepository.Verify((br => br.UpdateBlogPost(blogPost)), Times.Once());
        }
        #endregion

        #region DeleteBlogPost(int id) Tests
        [Test]
        public void DeleteBlogPost_should_call_BlogRepository_DeleteBlogPost()
        {
            int id = 0;

            blogger.DeleteBlogPost(id);

            mockBlogRepository.Verify((br => br.DeleteBlogPost(id)), Times.Once());
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

            mockBlogRepository.Verify((br => br.AddBlogPostVersion(Moq.It.IsAny<BlogVersionDto>())), Times.Once());
        }
        #endregion

        #region SaveBlogPostAsVersion(int blogPostId)
        [Test]
        public void SaveBlogPostAsVersion_int_should_return_an_int()
        {
            int blogPostId = blogPosts.ToList()[0].Id;
            int expectedResult = 1;

            mockBlogRepository.Setup(br => br.AddBlogPostVersion(Moq.It.IsAny<BlogVersionDto>())).Returns(expectedResult);
            mockBlogRepository.Setup(br => br.GetBlogPostById(blogPostId)).Returns(blogPosts.ToList()[0]);

            var result = blogger.SaveBlogPostAsVersion(blogPostId);

            Assert.IsInstanceOf<int>(result);
        }

        [Test]
        public void SaveBlogPostAsVersion_int_should_return_the_expected_int_value()
        {
            int blogPostId = blogPosts.ToList()[0].Id;
            int expectedResult = 3527;

            mockBlogRepository.Setup(br => br.AddBlogPostVersion(Moq.It.IsAny<BlogVersionDto>())).Returns(expectedResult);
            mockBlogRepository.Setup(br => br.GetBlogPostById(blogPostId)).Returns(blogPosts.ToList()[0]);

            var result = blogger.SaveBlogPostAsVersion(blogPostId);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void SaveBlogPostAsVersion_int_should_call_BlogRepository_AddBlogPostVersion()
        {
            int blogPostId = blogPosts.ToList()[0].Id;
            int expectedResult = 1;

            mockBlogRepository.Setup(br => br.AddBlogPostVersion(Moq.It.IsAny<BlogVersionDto>())).Returns(expectedResult);
            mockBlogRepository.Setup(br => br.GetBlogPostById(blogPostId)).Returns(blogPosts.ToList()[0]);

            var result = blogger.SaveBlogPostAsVersion(blogPostId);

            mockBlogRepository.Verify((br => br.AddBlogPostVersion(Moq.It.IsAny<BlogVersionDto>())), Times.Once());
            mockBlogRepository.Verify((br => br.GetBlogPostById(blogPostId)), Times.Once());
        }
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

            mockBlogRepository.Verify((br => br.UpdateBlogPostVersion(Moq.It.IsAny<BlogVersionDto>())), Times.Once());
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
        public void GetVersion_should_return_null_when_a_version_is_not_found()
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

            mockBlogRepository.Verify((br => br.GetBlogPostVersionById(Moq.It.IsAny<int>())), Times.Once());
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

            mockBlogRepository.Verify((br => br.GetBlogPostVersions()), Times.Once());
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

            mockBlogRepository.Verify((br => br.UpdateBlogPostVersion(version)), Times.Once());
        }
        #endregion

        #region DeleteVersion(int id) Tests
        [Test]
        public void DeleteVersion_should_call_BlogRepository_DeleteBlogPostVersion()
        {
            int id = 0;

            blogger.DeleteVersion(id);

            mockBlogRepository.Verify((br => br.DeleteBlogPostVersion(id)), Times.Once());
        }
        #endregion

        #region GetComment(int commentId)
        [Test]
        public void GetComment_should_return_an_object_of_type_BlogCommentDto()
        {
            int commentId = 1;

            mockBlogRepository.Setup(br => br.GetBlogCommentById(commentId)).Returns(blogComments.Where(v => v.Id == commentId).SingleOrDefault());

            var result = blogger.GetComment(commentId);

            Assert.IsInstanceOf<BlogCommentDto>(result);
        }

        [Test]
        public void GetComment_should_return_the_correct_comment()
        {
            int commentId = 1;

            mockBlogRepository.Setup(br => br.GetBlogCommentById(commentId)).Returns(blogComments.Where(v => v.Id == commentId).SingleOrDefault());

            var result = blogger.GetComment(commentId);

            Assert.IsInstanceOf<BlogCommentDto>(result);
        }

        [Test]
        public void GetComment_should_return_the_null_when_a_comment_is_not_found()
        {
            int commentId = 55;

            mockBlogRepository.Setup(br => br.GetBlogCommentById(commentId)).Returns(blogComments.Where(v => v.Id == commentId).SingleOrDefault());

            var result = blogger.GetComment(commentId);

            Assert.IsNull(result);
        }

        [Test]
        public void GetComment_should_call_BlogRepository_GetBlogCommentById_one_time()
        {
            int commentId = 27;

            mockBlogRepository.Setup(br => br.GetBlogCommentById(Moq.It.IsAny<int>()))
                .Returns((BlogCommentDto)null);

            var result = blogger.GetComment(commentId);

            mockBlogRepository.Verify((br => br.GetBlogCommentById(Moq.It.IsAny<int>())), Times.Once());
        }
        #endregion

        #region GetCommentsByBlogPost(int blogPostId, bool isApproved = true)
        [Test]
        public void GetCommentsByBlogPost_should_return_an_object_of_type_IEnumerable_BlogCommentDto()
        {
            int blogPostId = 1;

            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetCommentsByBlogPost(blogPostId);

            Assert.IsInstanceOf<IEnumerable<BlogCommentDto>>(result);
        }

        [Test]
        public void GetCommentsByBlogPost_should_return_the_correct_approved_blog_comments()
        {
            int blogPostId = 1;

            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetCommentsByBlogPost(blogPostId);

            Assert.AreEqual(2, result.Count(), "Wrong number of results");

            foreach (var comment in result)
            {
                Assert.AreEqual(blogPostId, comment.BlogPost.Id, "BlogPostId is not the same");
                Assert.IsTrue(comment.IsApproved, "Comment is not approved");
            }
        }

        [Test]
        public void GetCommentsByBlogPost_should_return_the_correct_approved_or_unapproved_blog_comments()
        {
            int blogPostId = 1;
            bool isApproved = false;

            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetCommentsByBlogPost(blogPostId, isApproved);

            Assert.AreEqual(3, result.Count(), "Wrong number of results");

            foreach (var comment in result)
            {
                Assert.AreEqual(blogPostId, comment.BlogPost.Id, "BlogPostId is not the same");
            }
        }

        [Test]
        public void GetCommentsByBlogPost_should_return_a_non_empty_collection()
        {
            int blogPostId = 1;

            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetCommentsByBlogPost(blogPostId);

            Assert.IsTrue(result.Count() > 0, "No results returned.");
        }

        [Test]
        public void GetCommentsByBlogPost_should_return_an_empty_collection()
        {
            int blogPostId = 55;

            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetCommentsByBlogPost(blogPostId);

            Assert.IsTrue(result.Count() == 0);
        }

        [Test]
        public void GetCommentsByBlogPost_should_return_the_list_of_comments_in_descending_order_by_CommentDate()
        {
            int blogPostId = 1;

            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetCommentsByBlogPost(blogPostId);

            Assert.IsTrue(result.Count() > 0, "No results returned.");

            bool isOrderedDescending = true;
            DateTime laterDate = DateTime.MaxValue;
            foreach (var version in result)
            {
                if (version.CommentDate > laterDate)
                {
                    isOrderedDescending = false;
                }
                laterDate = version.CommentDate;
            }
            Assert.IsTrue(isOrderedDescending, "Results are not in descending order by date modified.");
        }

        [Test]
        public void GetCommentsByBlogPost_should_call_BlogRepository_GetBlogComments_one_time()
        {
            int blogPostId = 27;

            mockBlogRepository.Setup(br => br.GetBlogComments())
                .Returns(blogComments);

            var result = blogger.GetCommentsByBlogPost(blogPostId);

            mockBlogRepository.Verify((br => br.GetBlogComments()), Times.Once());
        }
        #endregion
        
        #region GetCommentsByBlogPost(BlogPost blogPost, bool isApproved = true)
        [Test]
        public void GetCommentsByBlogPost_BlogPost_should_return_an_object_of_type_IEnumerable_BlogCommentDto()
        {
            BlogPostDto blogPost = blogPosts.ToList()[0];

            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetCommentsByBlogPost(blogPost);

            Assert.IsInstanceOf<IEnumerable<BlogCommentDto>>(result);
        }

        [Test]
        public void GetCommentsByBlogPost_BlogPost_should_return_the_correct_approved_blog_comments()
        {
            BlogPostDto blogPost = blogPosts.ToList()[0];

            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetCommentsByBlogPost(blogPost);

            Assert.AreEqual(2, result.Count(), "Wrong number of results");

            foreach (var comment in result)
            {
                Assert.AreEqual(blogPost.Id, comment.BlogPost.Id, "BlogPostId is not the same");
                Assert.IsTrue(comment.IsApproved, "Comment is not approved.");
            }
        }

        [Test]
        public void GetCommentsByBlogPost_BlogPost_should_return_the_correct_approved_or_unapproved_blog_comments()
        {
            BlogPostDto blogPost = blogPosts.ToList()[0];
            bool isApproved = false;

            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetCommentsByBlogPost(blogPost, isApproved);

            Assert.AreEqual(3, result.Count(), "Wrong number of results");

            foreach (var comment in result)
            {
                Assert.AreEqual(blogPost.Id, comment.BlogPost.Id, "BlogPostId is not the same");
            }
        }

        [Test]
        public void GetCommentsByBlogPost_BlogPost_should_return_a_non_empty_collection_of_blog_comments()
        {
            BlogPostDto blogPost = blogPosts.ToList()[0];

            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetCommentsByBlogPost(blogPost);

            Assert.IsTrue(result.Count() > 0, "Empty collection");
        }

        [Test]
        public void GetCommentsByBlogPost_BlogPost_should_return_an_empty_collection()
        {
            BlogPostDto blogPost = blogPosts.ToList()[6];

            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetCommentsByBlogPost(blogPost);

            Assert.IsTrue(result.Count() == 0);
        }

        [Test]
        public void GetCommentsByBlogPost_BlogPost_should_return_the_list_of_comments_in_descending_order_by_CommentDate()
        {
            BlogPostDto blogPost = blogPosts.ToList()[0];

            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetCommentsByBlogPost(blogPost);

            Assert.IsTrue(result.Count() > 0, "No results returned.");

            bool isOrderedDescending = true;
            DateTime laterDate = DateTime.MaxValue;
            foreach (var version in result)
            {
                if (version.CommentDate > laterDate)
                {
                    isOrderedDescending = false;
                }
                laterDate = version.CommentDate;
            }
            Assert.IsTrue(isOrderedDescending, "Results are not in descending order by date modified.");
        }

        [Test]
        public void GetCommentsByBlogPost_BlogPost_should_call_BlogRepository_GetBlogComments_one_time()
        {
            BlogPostDto blogPost = blogPosts.ToList()[0];

            mockBlogRepository.Setup(br => br.GetBlogComments())
                .Returns(blogComments);

            var result = blogger.GetCommentsByBlogPost(blogPost);

            mockBlogRepository.Verify((br => br.GetBlogComments()), Times.Once());
        }
        #endregion

        #region GetCommentCount(int blogPostId, bool isApproved = true)
        [Test]
        public void GetCommentCount_should_return_an_integer()
        {
            int blogPostId = 1;

            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetCommentCount(blogPostId);

            Assert.IsInstanceOf<int>(result);
        }

        [Test]
        public void GetCommentCount_should_return_the_correct_count_of_approved_comments()
        {
            int blogPostId = 1;
            bool isApproved = true;

            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetCommentCount(blogPostId, isApproved);

            Assert.AreEqual(2, result, "Wrong number of results");
        }

        [Test]
        public void GetCommentCount_should_return_the_correct_count_of_approved_or_unapproved_comments()
        {
            int blogPostId = 1;
            bool isApproved = false;

            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetCommentCount(blogPostId, isApproved);

            Assert.AreEqual(3, result, "Wrong number of results");
        }

        [Test]
        public void GetCommentCount_should_return_a_count_of_zero()
        {
            int blogPostId = 55;
            bool isApproved = false;

            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetCommentCount(blogPostId, isApproved);

            Assert.AreEqual(0, result, "Wrong number of results");
        }

        [Test]
        public void GetCommentCount_should_call_BlogRepository_GetBlogComments_one_time()
        {
            int blogPostId = 27;

            mockBlogRepository.Setup(br => br.GetBlogComments())
                .Returns(blogComments);

            var result = blogger.GetCommentCount(blogPostId);

            mockBlogRepository.Verify((br => br.GetBlogComments()), Times.Once());
        }
        #endregion

        #region GetUnapprovedComments() Tests
        [Test]
        public void GetUnapprovedComments_should_return_an_object_of_type_IEnumerable_BlogCommentDto()
        {
            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetUnapprovedComments();

            Assert.IsInstanceOf<IEnumerable<BlogCommentDto>>(result);
        }

        [Test]
        public void GetUnapprovedComments_should_return_a_non_empty_collection_of_comments()
        {
            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetUnapprovedComments();

            Assert.IsTrue(result.Count() > 0, "No results returned.");
        }

        [Test]
        public void GetUnapprovedComments_should_return_an_empty_collection_of_comments()
        {
            foreach (var comment in blogComments)
            {
                comment.IsApproved = true;
            }

            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetUnapprovedComments();

            Assert.IsTrue(result.Count() == 0, "Results were returned.");
        }

        [Test]
        public void GetUnapprovedComments_should_return_only_unapproved_comments()
        {
            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetUnapprovedComments();

            Assert.IsTrue(result.Count() > 0, "No results returned.");

            foreach (var comment in result)
            {
                Assert.IsFalse(comment.IsApproved);
            }
        }

        [Test]
        public void GetUnapprovedComments_should_call_BlogRepository_GetBlogComments_one_time()
        {
            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetUnapprovedComments();

            mockBlogRepository.Verify((br => br.GetBlogComments()), Times.Once());
        }
        #endregion

        #region GetUnapprovedComments(int blogPostId) Tests
        [Test]
        public void GetUnapprovedCommentsInt_should_return_an_object_of_type_IEnumerable_BlogCommentDto()
        {
            int blogPostId = 1;

            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetUnapprovedComments(blogPostId);

            Assert.IsInstanceOf<IEnumerable<BlogCommentDto>>(result);
        }

        [Test]
        public void GetUnapprovedCommentsInt_should_return_a_non_empty_collection_of_comments()
        {
            int blogPostId = 1;

            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetUnapprovedComments(blogPostId);

            Assert.IsTrue(result.Count() > 0, "No results returned.");
        }

        [Test]
        public void GetUnapprovedCommentsInt_should_return_an_empty_collection_of_comments()
        {
            int blogPostId = 1;

            foreach (var comment in blogComments)
            {
                comment.IsApproved = true;
            }

            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetUnapprovedComments(blogPostId);

            Assert.IsTrue(result.Count() == 0, "Results were returned.");
        }

        [Test]
        public void GetUnapprovedCommentsInt_should_return_only_unapproved_comments()
        {
            int blogPostId = 1;

            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetUnapprovedComments(blogPostId);

            Assert.IsTrue(result.Count() > 0, "No results returned.");

            foreach (var comment in result)
            {
                Assert.IsFalse(comment.IsApproved);
            }
        }

        [Test]
        public void GetUnapprovedCommentsInt_should_return_only_unapproved_comments_of_the_specified_blog_post()
        {
            int blogPostId = 3;

            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetUnapprovedComments(blogPostId);

            Assert.AreEqual(4, result.Count(), "Unexpected number of results.");

            foreach (var comment in result)
            {
                Assert.IsFalse(comment.IsApproved, "Comment is approved");
                Assert.AreEqual(blogPostId, comment.BlogPost.Id, "Comment with wrong blog post ID.");
            }
        }

        [Test]
        public void GetUnapprovedCommentsInt_should_call_BlogRepository_GetBlogComments_one_time()
        {
            int blogPostId = 3;

            mockBlogRepository.Setup(br => br.GetBlogComments()).Returns(blogComments);

            var result = blogger.GetUnapprovedComments(blogPostId);

            mockBlogRepository.Verify((br => br.GetBlogComments()), Times.Once());
        }
        #endregion

        #region ApproveComment(int commentId) Tests
        [Test]
        public void ApproveComment_should_call_BlogRepository_GetComment_and_UpdateComment_one_time_each()
        {
            int commentId = 2;

            mockBlogRepository.Setup(br => br.GetBlogCommentById(commentId))
                .Returns(blogComments.Where(v => v.Id == commentId).SingleOrDefault());
            mockBlogRepository.Setup(br => br.UpdateBlogComment(Moq.It.IsAny<BlogCommentDto>()));

            blogger.ApproveComment(commentId);

            mockBlogRepository.Verify((br => br.GetBlogCommentById(commentId)), Times.Once());
            mockBlogRepository.Verify((br => br.UpdateBlogComment(Moq.It.IsAny<BlogCommentDto>())), Times.Once());
        }

        [Test]
        public void ApproveComment_should_call_BlogRepository_GetComment_one_time_and_UpdateComment_zero_times_comment_already_approved()
        {
            int commentId = 1;

            mockBlogRepository.Setup(br => br.GetBlogCommentById(commentId))
                .Returns(blogComments.Where(v => v.Id == commentId).SingleOrDefault());
            mockBlogRepository.Setup(br => br.UpdateBlogComment(Moq.It.IsAny<BlogCommentDto>()));

            blogger.ApproveComment(commentId);

            mockBlogRepository.Verify((br => br.GetBlogCommentById(commentId)), Times.Once());
            mockBlogRepository.Verify((br => br.UpdateBlogComment(Moq.It.IsAny<BlogCommentDto>())), Times.Never());
        }

        [Test]
        public void ApproveComment_should_call_BlogRepository_GetComment_and_UpdateComment_one_time_each_to_unapprove_a_comment()
        {
            int commentId = 1;
            bool isApproved = false;

            mockBlogRepository.Setup(br => br.GetBlogCommentById(commentId))
                .Returns(blogComments.Where(v => v.Id == commentId).SingleOrDefault());
            mockBlogRepository.Setup(br => br.UpdateBlogComment(Moq.It.IsAny<BlogCommentDto>()));

            blogger.ApproveComment(commentId, isApproved);

            mockBlogRepository.Verify((br => br.GetBlogCommentById(commentId)), Times.Once());
            mockBlogRepository.Verify((br => br.UpdateBlogComment(Moq.It.IsAny<BlogCommentDto>())), Times.Once());
        }

        [Test]
        public void ApproveComment_should_call_BlogRepository_GetComment_one_time_and_UpdateComment_zero_times_to_comment_already_unapproved()
        {
            int commentId = 2;
            bool isApproved = false;

            mockBlogRepository.Setup(br => br.GetBlogCommentById(commentId))
                .Returns(blogComments.Where(v => v.Id == commentId).SingleOrDefault());
            mockBlogRepository.Setup(br => br.UpdateBlogComment(Moq.It.IsAny<BlogCommentDto>()));

            blogger.ApproveComment(commentId, isApproved);

            mockBlogRepository.Verify((br => br.GetBlogCommentById(commentId)), Times.Once());
            mockBlogRepository.Verify((br => br.UpdateBlogComment(Moq.It.IsAny<BlogCommentDto>())), Times.Never());
        }
        #endregion

        #region AddComment(BlogCommentDto comment) Tests
        [Test]
        public void AddComment_should_return_an_int()
        {
            BlogCommentDto comment = new BlogCommentDto { Id = 1 };

            mockBlogRepository.Setup(br => br.AddBlogComment(comment)).Returns(comment.Id);

            var result = blogger.AddComment(comment);

            Assert.IsInstanceOf<int>(result);
        }

        [Test]
        public void AddComment_should_return_the_correct_id_number()
        {
            BlogCommentDto comment = new BlogCommentDto { Id = 1 };

            mockBlogRepository.Setup(br => br.AddBlogComment(comment)).Returns(comment.Id);

            var result = blogger.AddComment(comment);

            Assert.AreEqual(comment.Id, result);
        }

        [Test]
        public void AddComment_should_call_BlogRepository_AddBlogComment()
        {
            BlogCommentDto comment = new BlogCommentDto();

            blogger.AddComment(comment);

            mockBlogRepository.Verify((br => br.AddBlogComment(comment)), Times.Once());
        }
        #endregion

        #region UpdateComment(BlogCommentDto comment) Tests
        [Test]
        public void UpdateComment_should_call_BlogRepository_UpdateBlogComment()
        {
            BlogCommentDto comment = new BlogCommentDto();

            blogger.UpdateComment(comment);

            mockBlogRepository.Verify((br => br.UpdateBlogComment(comment)), Times.Once());
        }
        #endregion

        #region DeleteComment(int id) Tests
        [Test]
        public void DeleteComment_should_call_BlogRepository_DeleteBlogComment()
        {
            int commentId = 0;

            blogger.DeleteComment(commentId);

            mockBlogRepository.Verify((br => br.DeleteBlogComment(commentId)), Times.Once());
        }
        #endregion
    }
}
