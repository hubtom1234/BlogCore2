using Blog.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Infrastructure
{
    public class BlogContext : DbContext
    {
        private string _connectionString;
        private string _defaultConnection = "Data Source=localhost\\MSSQLSERVER01;Initial Catalog=C:\\tmp\\blog-test.mdf;TrustServerCertificate=True;Integrated Security=SSPI";
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public BlogContext(string connectionString) : base()
        {
            _connectionString = connectionString;
        }

        public BlogContext() : base()
        {
            _connectionString = _defaultConnection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }
    }
}
