using Microsoft.EntityFrameworkCore;
using Rule.DAL.Entities;

namespace Rule.DAL.Context
{
    public class ApiDbContext: DbContext
    {
        public virtual DbSet<Foundations> Foundations { get; set; }
        public virtual DbSet<Posts> Posts { get; set; }
        public virtual DbSet<StatusPost> StatusPosts { get; set; }
        public virtual DbSet<TypePost> TypePosts { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }
    }
}
