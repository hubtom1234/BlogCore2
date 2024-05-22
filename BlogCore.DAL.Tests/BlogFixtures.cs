using Blog.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.DbTestHelpers.Yaml;

namespace BlogCore.DAL.Tests
{
    public class BlogFixtures
 : YamlDbFixture<BlogContext, BlogFixturesModel>
    {
        public BlogFixtures()
        {
            SetYamlFolderName("C:\\Users\\hubto\\Desktop\\SAB\\lab1\\BlogCore\\BlogCore.DAL.Tests\\Fixtures\\");
            SetYamlFiles("posts.yml");
        }
    }

}
