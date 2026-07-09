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
    }
}
