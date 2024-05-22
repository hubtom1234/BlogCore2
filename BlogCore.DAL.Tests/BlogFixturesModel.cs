using Blog.DAL.Model;
using TDD.DbTestHelpers.Yaml;

namespace BlogCore.DAL.Tests
{
    public class BlogFixturesModel
    {
        public FixtureTable<Post> Posts { get; set; }
        public FixtureTable<Comment> Comments { get; set; }
    }
}