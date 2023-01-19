using DataSaver.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataSaver.Infrastructure.Data
{
    public sealed class TopicContext : DbContext
    {
        public DbSet<Topic> Topics { get; set; }

        public DbSet<Link> Links { get; set; }

        public DbSet<Category> Categories { get; set; }

        public TopicContext(DbContextOptions<TopicContext> options) : base(options)
        {
        }
    }
}
