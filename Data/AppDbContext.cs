using Cards.Enums;
using Microsoft.EntityFrameworkCore;
using Cards.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;

namespace Cards
{
    public class AppDbContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>(options);
            optionsBuilder.UseSqlServer(
                optionsBuilder.Options!.FindExtension<SqlServerOptionsExtension>()!.ConnectionString,
                providerOptions => providerOptions.EnableRetryOnFailure());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}