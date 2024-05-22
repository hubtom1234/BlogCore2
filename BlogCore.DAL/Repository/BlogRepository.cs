using System.Collections.Generic;
using Blog.DAL.Infrastructure;
using Blog.DAL.Model;
using System;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Repository
{
    public class BlogRepository
    {
        private readonly BlogContext _context;

        public BlogRepository(string connectionString)
        {
            _context = new BlogContext(connectionString);
        }

        public void ClearAll()
        {
            _context.Posts.ExecuteDelete();
            _context.Comments.ExecuteDelete();
        }

        public void AddPost(int id, string content, string author)
        {
            Post post = new Post {Id = id, Content = content, Author = author };
            _context.Posts.Add(post);
            _context.SaveChanges();
        }
        public void AddComment(int id, int postId, string content)
        {
            Comment comment = new Comment { Id = id, Content = content, PostId = postId };
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }
        public IEnumerable<Post> GetAllPosts()
        {
            return _context.Posts;
        }
        public IEnumerable<Comment> GetAllComments()
        {
            return _context.Comments;
        }

        public IEnumerable<Comment> GetPostComments(Post post)
        {
            return _context.Comments.Where(c => c.PostId == post.Id);
        }
    }
}
