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
        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Note> Notes { get; set; } = null!;

        public DbSet<RecentVideo> RecentYTVideos { get; set; } = null!;

        public DbSet<RecentPDF> RecentPDFs { get; set; } = null!;

        public DbSet<EmailVerification> EmailVerifications { get; set; } = null!;

        public DbSet<PasswordReset> PasswordResets { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecentVideo>()
                .ToTable("RecentYTVideos_tb");

            modelBuilder.Entity<RecentPDF>()
        .ToTable("PDFHistory_tb");

            modelBuilder.Entity<RecentPDF>()
      .HasKey(x => x.PdfId);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PasswordReset>()
       .ToTable("PasswordReset_tb");

            modelBuilder.Entity<PasswordReset>()
      .HasKey(x => x.ResetId);


            modelBuilder.Entity<PasswordReset>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<EmailVerification>()
    .ToTable("EmailVerification_tb");

        }

    }
}
