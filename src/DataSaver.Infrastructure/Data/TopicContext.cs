using DataSaver.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataSaver.Infrastructure.Data
{
    public sealed class TopicContext : DbContext
    {
        public TopicContext(DbContextOptions<TopicContext> options) : base(options)
        {
        }

        public DbSet<Topic> Topic { get; set; } 
        public DbSet<TopicItem> TopicItem { get; set; }
    }
}
