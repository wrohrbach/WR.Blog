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
    public class BlogServiceTest
    {
        private Mock<IBlogRepository> mockBlogRepository;
        private IQueryable<BlogPage> blogPages;
        private BlogService blogger;

        #region Setup Methods
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            blogPages = new List<BlogPage>{
                new BlogPage {
                    Id = 1,
                    Title = "Title 1",
                    UrlSegment = "title-1",
                    PublishedDate = new DateTime(2013, 3, 1),
                    IsPublished = true,
                    IsContentPage = true
                },
                new BlogPage {
                    Id = 2,
                    Title = "Title 2",
                    PublishedDate = new DateTime(2013, 2, 1),
                    IsPublished = true,
                    IsContentPage = false
                },
                new BlogPage {
                    Id = 3,
                    Title = "Title 3",
                    UrlSegment = "title-3",
                    PublishedDate = new DateTime(2013, 3, 3),
                    IsPublished = true,
                    IsContentPage = false
                },
                new BlogPage {
                    Id = 4,
                    Title = "Title 4",
                    UrlSegment = "title-4",
                    PublishedDate = new DateTime(2013, 3, 5),
                    IsPublished = false,
                    IsContentPage = false
                },
                new BlogPage {
                    Id = 5,
                    Title = "Title 5",
                    PublishedDate = new DateTime(2013, 3, 17),
                    IsPublished = true,
                    IsContentPage = false
                },
                new BlogPage {
                    Id = 6,
                    Title = "Title 6",
                    PublishedDate = new DateTime(2012, 3, 17),
                    IsPublished = true,
                    IsContentPage = false
                },
                new BlogPage {
                    Id = 7,
                    Title = "Title 7",
                    UrlSegment = "title-7",
                    PublishedDate = new DateTime(2013, 3, 3, 23, 59, 58),
                    IsPublished = true,
                    IsContentPage = false
                }
            }.AsQueryable();
        }

        [SetUp]
        public void Setup()
        {
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

        #region GetBlogPagesByDate() Tests
        [Test]
        public void GetBlogPagesByDate_should_return_an_object_type_of_IEnumerable_BlogPage()
        {
            int? year = 2010;
            int? month = 1;
            int? day = 5;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPagesByDate(year, month, day);

            Assert.IsInstanceOf(typeof(IEnumerable<BlogPage>), result);
        }

        [Test]
        public void GetBlogPagesByDate_should_return_blog_pages_published_on_specific_day()
        {
            int year = 2013;
            int month = 3;
            int day = 5;
            DateTime date = new DateTime(2013, month, day);

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPagesByDate(year, month, day);

            Assert.AreNotEqual(null, result);
            foreach (var blogPage in result)
            {
                Assert.AreEqual(date.Date, blogPage.PublishedDate.Date);
            }
        }

        [Test]
        public void GetBlogPagesByDate_should_return_blog_pages_published_during_specific_month()
        {
            int year = 2013;
            int month = 3;
            int? day = null;
            DateTime dateFrom = new DateTime(year, month, 1);
            DateTime dateTo = dateFrom.AddMonths(1).AddSeconds(-1);

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPagesByDate(year, month, day);

            Assert.AreNotEqual(null, result);
            foreach (var blogPage in result)
            {
                Assert.LessOrEqual(dateFrom, blogPage.PublishedDate);
                Assert.GreaterOrEqual(dateTo, blogPage.PublishedDate);
            }
        }

        [Test]
        public void GetBlogPagesByDate_should_return_blog_pages_published_during_specific_year()
        {
            int year = 2013;
            int? month = null;
            int? day = null;
            DateTime dateFrom = new DateTime(year, 1, 1);
            DateTime dateTo = new DateTime(year + 1, 1, 1).AddSeconds(-1);

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPagesByDate(year, month, day);

            Assert.AreNotEqual(null, result);
            foreach (var blogPage in result)
            {
                Assert.LessOrEqual(dateFrom, blogPage.PublishedDate);
                Assert.GreaterOrEqual(dateTo, blogPage.PublishedDate);
            }
        }

        [Test]
        public void GetBlogPagesByDate_should_return_zero_blog_pages_for_specific_date()
        {
            int year = 2013;
            int? month = 1;
            int? day = 1;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPagesByDate(year, month, day);

            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void GetBlogPagesByDate_should_return_zero_blog_pages_for_specific_month()
        {
            int year = 2013;
            int? month = 1;
            int? day = null;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPagesByDate(year, month, day);

            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void GetBlogPagesByDate_should_return_zero_blog_pages_for_specific_year()
        {
            int year = 2010;
            int? month = null;
            int? day = null;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPagesByDate(year, month, day);

            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void GetBlogPagesByDate_should_return_only_blog_pages_where_IsPublished_is_true()
        {
            int year = 2013;
            int? month = null;
            int? day = null;
            bool isPublished = true;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPagesByDate(year, month, day, isPublished);

            bool onlyPublished = true;
            foreach (var blogPage in result)
            {
                if (!blogPage.IsPublished)
                {
                    onlyPublished = false;
                }
            }
            Assert.IsTrue(onlyPublished);
        }

        [Test]
        public void GetBlogPagesByDate_should_return_only_blog_pages_whether_IsPublished_is_true_or_false()
        {
            int year = 2013;
            int? month = null;
            int? day = null;
            bool isPublished = false;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPagesByDate(year, month, day, isPublished);

            bool onlyPublished = true;
            bool onlyUnpublished = true;
            foreach (var blogPage in result)
            {
                if (!blogPage.IsPublished)
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
        public void GetBlogPagesByDate_should_return_only_blog_pages_where_IsContentPage_is_false()
        {
            int year = 2013;
            int? month = null;
            int? day = null;
            bool isPublished = false;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPagesByDate(year, month, day, isPublished);

            if (blogPages.Where(bp => bp.IsContentPage && bp.PublishedDate.Year == 2013).FirstOrDefault() == null)
            {
                throw new Exception("No Content Pages to test against.");
            }

            bool hasContentPages = false;
            foreach (var blogPage in result)
            {
                if (blogPage.IsContentPage)
                {
                    hasContentPages = true;
                }
            }
            Assert.IsFalse(hasContentPages);
        }

        [Test]
        public void GetBlogPagesByDate_should_return_null_based_on_year_is_null()
        {
            int? year = null;
            int? month = 3;
            int? day = 1;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPagesByDate(year, month, day);

            Assert.AreEqual(null, result);
        }

        [Test]
        public void GetBlogPagesByDate_should_return_null_based_on_year_month_day_are_null()
        {
            int? year = null;
            int? month = null;
            int? day = null;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPagesByDate(year, month, day);

            Assert.AreEqual(null, result);
        }

        [Test]
        public void GetBlogPagesByDate_should_not_call_BlogRepository_GetBlogPages_based_on_year_is_null()
        {
            int? year = null;
            int? month = 3;
            int? day = 1;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPagesByDate(year, month, day);

            mockBlogRepository.Verify((br => br.GetBlogPages()), Times.Never());
        }

        [Test]
        public void GetBlogPagesByDate_should_not_call_BlogRepository_GetBlogPages_based_on_year_month_day_are_null()
        {
            int? year = null;
            int? month = null;
            int? day = null;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPagesByDate(year, month, day);

            mockBlogRepository.Verify((br => br.GetBlogPages()), Times.Never());
        }

        [Test]
        public void GetBlogPagesByDate_should_not_call_BlogRepository_GetBlogPages_based_on_invalid_year()
        {
            int? year = 10001;
            int? month = 1;
            int? day = 1;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPagesByDate(year, month, day);

            mockBlogRepository.Verify((br => br.GetBlogPages()), Times.Never());
        }

        [Test]
        public void GetBlogPagesByDate_should_not_call_BlogRepository_GetBlogPages_based_on_invalid_month()
        {
            int? year = 2010;
            int? month = 13;
            int? day = 1;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPagesByDate(year, month, day);

            mockBlogRepository.Verify((br => br.GetBlogPages()), Times.Never());
        }

        [Test]
        public void GetBlogPagesByDate_should_not_call_BlogRepository_GetBlogPages_based_on_invalid_day()
        {
            int? year = 2010;
            int? month = 2;
            int? day = 29;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPagesByDate(year, month, day);

            mockBlogRepository.Verify((br => br.GetBlogPages()), Times.Never());
        }

        [Test]
        public void GetBlogPagesByDate_should_call_BlogRepository_GetBlogPages_based_on_year_month_day_are_not_null()
        {
            int? year = 2010;
            int? month = 1;
            int? day = 5;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPagesByDate(year, month, day);

            mockBlogRepository.Verify((br => br.GetBlogPages()), Times.Exactly(1));
        }
        #endregion

        #region GetBlogPage(int id) Tests
        [Test]
        public void GetBlogPageInt_should_return_an_object_of_type_BlogPage()
        {
            int id = 5;

            mockBlogRepository.Setup(br => br.GetBlogPageById(id)).Returns(blogPages.Where(b => b.Id == id).SingleOrDefault());

            var result = blogger.GetBlogPage(id);

            Assert.IsInstanceOf(typeof(BlogPage), result);
        }

        [Test]
        public void GetBlogPageInt_should_return_the_correct_BlogPage()
        {
            int id = 5;

            mockBlogRepository.Setup(br => br.GetBlogPageById(id)).Returns(blogPages.Where(b => b.Id == id).SingleOrDefault());

            var result = blogger.GetBlogPage(id);

            Assert.AreEqual(id, result.Id);
        }
        #endregion

        #region GetBlogPages(int? skip, int? take, bool published, bool content) Tests
        [Test]
        public void GetBlogPagesIntIntBoolBoolBool_should_return_an_object_of_type_IEnumerable_BlogPage()
        {
            int? skip = null;
            int? take = null;
            bool published = false;
            bool content = false;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPages(skip, take, published, content);

            Assert.IsInstanceOf(typeof(IEnumerable<BlogPage>), result);
        }

        [Test]
        public void GetBlogPagesIntIntBoolBoolBool_should_return_two_BlogPages()
        {
            int? skip = null;
            int? take = 2;
            bool published = false;
            bool content = false;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPages(skip, take, published, content);

            Assert.AreEqual(take, result.Count());
        }

        [Test]
        public void GetBlogPagesIntIntBoolBoolBool_should_not_skip_without_a_take_value()
        {
            int? skip = 2;
            int? take = null;
            bool published = false;
            bool content = true;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPages(skip, take, published, content);

            Assert.AreEqual(blogPages.Count(), result.Count());
        }

        [Test]
        public void GetBlogPagesIntIntBoolBoolBool_should_skip_4_and_take_2()
        {
            int? skip = 3; // skipValue = (skip - 1) * take;
            int? take = 2;
            bool published = false;
            bool content = true;
            bool orderDescending = false;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPages(skip, take, published, content, orderDescending);
            var expected = blogPages.Skip(4).Take(2).ToList();

            int counter = 0;
            foreach (var page in result)
            {
                Assert.AreEqual(expected[counter++].Id, page.Id);
            }
        }

        [Test]
        public void GetBlogPagesIntIntBoolBoolBool_should_return_published_and_unpublished_pages()
        {
            int? skip = null; // skipValue = (skip - 1) * take;
            int? take = null;
            bool published = false;
            bool content = true;
            bool orderDescending = false;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPages(skip, take, published, content, orderDescending);

            bool unpublishedFound = false;
            foreach (var page in result)
            {
                if (!page.IsPublished)
                {
                    unpublishedFound = true;
                }
            }
            Assert.IsTrue(unpublishedFound);
        }

        [Test]
        public void GetBlogPagesIntIntBoolBoolBool_should_return_only_published_pages()
        {
            int? skip = null; // skipValue = (skip - 1) * take;
            int? take = null;
            bool published = true;
            bool content = true;
            bool orderDescending = false;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPages(skip, take, published, content, orderDescending);

            foreach (var page in result)
            {
                Assert.IsTrue(page.IsPublished);
            }
        }

        [Test]
        public void GetBlogPagesIntIntBoolBoolBool_should_include_content_pages()
        {
            int? skip = null; // skipValue = (skip - 1) * take;
            int? take = null;
            bool published = true;
            bool content = true;
            bool orderDescending = false;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPages(skip, take, published, content, orderDescending);

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
        public void GetBlogPagesIntIntBoolBoolBool_should_not_include_content_pages()
        {
            int? skip = null; // skipValue = (skip - 1) * take;
            int? take = null;
            bool published = true;
            bool content = false;
            bool orderDescending = false;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPages(skip, take, published, content, orderDescending);

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
        public void GetBlogPagesIntIntBoolBoolBool_should_order_by_descending_published_date()
        {
            int? skip = null; // skipValue = (skip - 1) * take;
            int? take = null;
            bool published = false;
            bool content = true;
            bool orderDescending = true;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPages(skip, take, published, content, orderDescending);

            bool isOrderedDescending = true;
            DateTime laterDate = DateTime.MaxValue;
            foreach (var page in result)
            {
                if (page.PublishedDate > laterDate)
                {
                    isOrderedDescending = false;
                }
                laterDate = page.PublishedDate;
            }
            Assert.IsTrue(isOrderedDescending);
        }

        [Test]
        public void GetBlogPagesIntIntBoolBoolBool_should_not_order_by_descending_published_date()
        {
            int? skip = null; // skipValue = (skip - 1) * take;
            int? take = null;
            bool published = false;
            bool content = true;
            bool orderDescending = false;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPages(skip, take, published, content, orderDescending);

            bool isOrderedDescending = true;
            DateTime laterDate = DateTime.MaxValue;
            foreach (var page in result)
            {
                if (page.PublishedDate > laterDate)
                {
                    isOrderedDescending = false;
                }
                laterDate = page.PublishedDate;
            }
            Assert.IsFalse(isOrderedDescending);
        }

        [Test]
        public void GetBlogPagesIntIntBoolBoolBool_should_return_all_blog_pages()
        {
            int? skip = null; // skipValue = (skip - 1) * take;
            int? take = null;
            bool published = false;
            bool content = true;
            bool orderDescending = false;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPages(skip, take, published, content, orderDescending);

            Assert.AreEqual(blogPages.Count(), result.Count());
        }

        [Test]
        public void GetBlogPagesIntIntBoolBoolBool_should_call_BlogRepository_GetBlogPages()
        {
            int? skip = null;
            int? take = null;
            bool published = false;
            bool content = false;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPages(skip, take, published, content);

            mockBlogRepository.Verify((br => br.GetBlogPages()), Times.Exactly(1));
        }
        #endregion

        #region GetBlogPageByUrlSegment(string urlSegment, bool isContentPage = false) Tests
        [Test]
        public void GetBlogPageByUrlSegmentStringBool_should_return_an_object_of_type_BlogPage()
        {
            string urlSegment = "title-1";
            bool isContentPage = true;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPageByUrlSegment(urlSegment, isContentPage);

            Assert.IsInstanceOf(typeof(BlogPage), result);
        }

        [Test]
        public void GetBlogPageByUrlSegmentStringBool_should_return_null_no_url_segment_match()
        {
            string urlSegment = "not a url segment";
            bool isContentPage = true;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPageByUrlSegment(urlSegment, isContentPage);

            Assert.IsNull(result);
        }

        [Test]
        public void GetBlogPageByUrlSegmentStringBool_should_not_return_null_url_segment_match_but_not_content_page()
        {
            string urlSegment = "title-7";
            bool isContentPage = false;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPageByUrlSegment(urlSegment, isContentPage);

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetBlogPageByUrlSegmentStringBool_should_return_a_matching_url_segment()
        {
            string urlSegment = "title-7";
            bool isContentPage = false;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPageByUrlSegment(urlSegment, isContentPage);

            Assert.AreEqual(urlSegment, result.UrlSegment);
        }

        [Test]
        public void GetBlogPageByUrlSegmentStringBool_should_return_a_matching_url_segment_and_content_page()
        {
            string urlSegment = "title-1";
            bool isContentPage = true;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPageByUrlSegment(urlSegment, isContentPage);

            Assert.AreEqual(urlSegment, result.UrlSegment);
            Assert.IsTrue(result.IsContentPage);
        }

        [Test]
        public void GetBlogPageByUrlSegmentStringBool_should_return_a_matching_url_segment_even_though_not_specified_as_content_page()
        {
            string urlSegment = "title-1";
            bool isContentPage = false;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPageByUrlSegment(urlSegment, isContentPage);

            Assert.AreEqual(urlSegment, result.UrlSegment);
        }

        [Test]
        public void GetBlogPageByUrlSegmentStringBool_should_call_BlogRepository_GetBlogPages()
        {
            string urlSegment = "title-1";
            bool isContentPage = false;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPageByUrlSegment(urlSegment, isContentPage);

            mockBlogRepository.Verify((br => br.GetBlogPages()), Times.Exactly(1));
        }
        #endregion

        #region GetBlogPageByPermalink(int? year, int? month, int? day, string urlSegment, bool isPublished = true) Tests
        [Test]
        public void GetBlogPageByPermalink_should_return_an_object_of_type_BlogPage()
        {
            int year = 2013;
            int month = 3;
            int day = 3;
            string urlSegment = "title-3";
            bool isPublished = true;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPageByPermalink(year, month, day, urlSegment, isPublished);

            Assert.IsInstanceOf(typeof(BlogPage), result);
        }

        [Test]
        public void GetBlogPageByPermalink_should_return_null_because_attempted_match_is_content_page()
        {
            int year = 2013;
            int month = 3;
            int day = 1;
            string urlSegment = "title-1";
            bool isPublished = true;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPageByPermalink(year, month, day, urlSegment, isPublished);

            Assert.IsNull(result);
        }

        [Test]
        public void GetBlogPageByPermalink_should_return_null_because_no_UrlSegment_match()
        {
            int year = 2013;
            int month = 3;
            int day = 3;
            string urlSegment = "Not a url segment";
            bool isPublished = true;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPageByPermalink(year, month, day, urlSegment, isPublished);

            Assert.IsNull(result);
        }

        [Test]
        public void GetBlogPageByPermalink_should_return_null_because_matched_UrlSegment_has_IsPublished_flag_set_to_false()
        {
            int year = 2013;
            int month = 3;
            int day = 5;
            string urlSegment = "title-4";
            bool isPublished = true;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPageByPermalink(year, month, day, urlSegment, isPublished);

            Assert.IsNull(result);
        }

        [Test]
        public void GetBlogPageByPermalink_should_match_url_segment_and_IsPublished_flag_is_true()
        {
            int year = 2013;
            int month = 3;
            int day = 3;
            string urlSegment = "title-3";
            bool isPublished = true;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPageByPermalink(year, month, day, urlSegment, isPublished);

            Assert.IsNotNull(result);
            Assert.AreEqual(urlSegment, result.UrlSegment);
        }

        [Test]
        public void GetBlogPageByPermalink_should_match_url_segment_and_IsPublished_flag_is_false()
        {
            int year = 2013;
            int month = 3;
            int day = 5;
            string urlSegment = "title-4";
            bool isPublished = false;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPageByPermalink(year, month, day, urlSegment, isPublished);

            Assert.IsNotNull(result);
            Assert.AreEqual(urlSegment, result.UrlSegment);
        }

        [Test]
        public void GetBlogPageByPermalink_should_call_BlogRepository_GetBlogPages()
        {
            int year = 2013;
            int month = 3;
            int day = 5;
            string urlSegment = "title-4";
            bool isPublished = false;

            mockBlogRepository.Setup(br => br.GetBlogPages()).Returns(blogPages);

            var result = blogger.GetBlogPageByPermalink(year, month, day, urlSegment, isPublished);

            mockBlogRepository.Verify((br => br.GetBlogPages()), Times.Exactly(1));
        }
        #endregion

        #region AddBlogPage(BlogPage blogPage) Tests
        [Test]
        public void AddBlogPage_should_call_BlogRepository_AddBlogPage()
        {
            BlogPage blogPage = new BlogPage();

            blogger.AddBlogPage(blogPage);

            mockBlogRepository.Verify((br => br.AddBlogPage(blogPage)), Times.Exactly(1));
        }
        #endregion

        #region UpdateBlogPage(BlogPage blogPage) Tests
        [Test]
        public void UpdateBlogPage_should_call_BlogRepository_UpdateBlogPage()
        {
            BlogPage blogPage = new BlogPage();

            blogger.UpdateBlogPage(blogPage);

            mockBlogRepository.Verify((br => br.UpdateBlogPage(blogPage)), Times.Exactly(1));
        }
        #endregion

        #region DeleteBlogPage(int id) Tests
        [Test]
        public void DeletelogPage_should_call_BlogRepository_DeleteBlogPage()
        {
            int id = 0;

            blogger.DeleteBlogPage(id);

            mockBlogRepository.Verify((br => br.DeleteBlogPage(id)), Times.Exactly(1));
        }
        #endregion
    }
}
