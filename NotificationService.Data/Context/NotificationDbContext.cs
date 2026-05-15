using Microsoft.EntityFrameworkCore;
using NotificationService.Repository.Entities;

namespace NotificationService.Data.Context
{
    public class NotificationDbContext : DbContext
    {
        public DbSet<Notification> notifications { get; set; }

        public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Notification>(entity =>
            {
                entity.Property(p => p.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");

            });
        }
    }     
}
