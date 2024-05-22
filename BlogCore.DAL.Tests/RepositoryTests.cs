using Blog.DAL.Infrastructure;
using Blog.DAL.Repository;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using ConfigurationManager = System.Configuration.ConfigurationManager;
using Blog.DAL.Model;
using TDD.DbTestHelpers.Yaml;
using BlogCore.DAL.Tests;
using TDD.DbTestHelpers.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using YamlDotNet.Core.Tokens;

namespace Blog.DAL.Tests
{
    [TestClass]
    public class RepositoryTests
    {
        public class TestDbTest : DbBaseTest<BlogFixtures>
        {
            public TestDbTest() : base() { }
        }
        public static string GetConnectionString(string name)
        {
            Configuration config =
            ConfigurationManager.OpenExeConfiguration(
            ConfigurationUserLevel.None);
            ConnectionStringsSection csSection =
            config.ConnectionStrings;
            for (int i = 0; i <
            ConfigurationManager.ConnectionStrings.Count; i++)
            {
                ConnectionStringSettings cs =
                csSection.ConnectionStrings[i];
                if (cs.Name == name)
                {
                    return cs.ConnectionString;
                }
            }
            return "";
        }

        [TestMethod]
        public void GetAllPost_ReadTwoPosts_ReturnTwoPosts()
        {
            String connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("BloggingDatabase");
            var repository = new BlogRepository(connectionString);
            repository.ClearAll();

            TestDbTest dbTest = new TestDbTest();
            // arrange

            var context = dbTest.fixContext();

            dbTest.BaseFixtureSetUp();
            dbTest.BaseSetUp();
            context.Database.EnsureCreated();
            // act
            var result = repository.GetAllPosts();
            // assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void AddPostReturnPlusOne()
        {
            String connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("BloggingDatabase");
            var repository = new BlogRepository(connectionString);
            repository.ClearAll();

            var countBefore = repository.GetAllPosts().Count();
            repository.AddPost(3, "test", "author");
            var countAfter = repository.GetAllPosts().Count();

            Assert.AreEqual(countBefore + 1, countAfter);
        }

        [TestMethod]
        [ExpectedException(typeof(Microsoft.EntityFrameworkCore.DbUpdateException))]
        public void CantAddPostWithEmptyAuthor()
        {
            String connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("BloggingDatabase");
            var repository = new BlogRepository(connectionString);
            repository.ClearAll();

            TestDbTest dbTest = new TestDbTest();
            dbTest.BaseTearDown();

            repository.AddPost(4, "test", null);
        }

        [TestMethod]
        public void ReadCommentReturnOneComment()
        {
            String connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("BloggingDatabase");
            var repository = new BlogRepository(connectionString);
            repository.ClearAll();

            TestDbTest dbTest = new TestDbTest();

            // arrange
        
            var context = dbTest.fixContext();
        
            dbTest.BaseFixtureSetUp();
            dbTest.BaseSetUp();
            context.Database.EnsureCreated();

            // act
            var result = repository.GetAllComments();
            // assert
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void AddComment()
        {
            String connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("BloggingDatabase");
            var repository = new BlogRepository(connectionString);
            repository.ClearAll();
            // arrange

            var countBefore = repository.GetAllComments().Count();

            repository.AddPost(1, "some", "data");
            repository.AddComment(12, 1, "commmm");

            var countAfter = repository.GetAllComments().Count();

            Assert.AreEqual(countBefore + 1, countAfter);
        }

        [TestMethod]
        public void RetrieveOnlySelected()
        {
            String connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("BloggingDatabase");
            var repository = new BlogRepository(connectionString);
            repository.ClearAll();

            repository.AddPost(1, "some", "data");
            repository.AddComment(1, 1, "commmm");
            repository.AddPost(2, "other", "data");
            repository.AddComment(2, 2, "commmm");
            repository.AddComment(3, 2, "commmm");
            repository.AddPost(3, "another", "data");
            repository.AddComment(4, 3, "commmm");
            repository.AddComment(5, 3, "commmm");
            repository.AddComment(6, 3, "commmm");

            Post selectedPost = repository.GetAllPosts().FirstOrDefault(p => p.Id == 2);
            var countSelected = repository.GetPostComments(selectedPost).Count();

            Assert.AreEqual(countSelected, 2);
        }
    }
}
