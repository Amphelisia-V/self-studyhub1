using Microsoft.EntityFrameworkCore;
using SelfStudyHub2.API.Models;

namespace SelfStudyHub2.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {

        }
        public DbSet<User> Users { get; set; }

        public DbSet<Note> Notes { get; set; }

        public DbSet<RecentVideo> RecentYTVideos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           


            modelBuilder.Entity<RecentVideo>()
                .ToTable("RecentYTVideos_tb");
            base.OnModelCreating(modelBuilder);
        }
    }
}
