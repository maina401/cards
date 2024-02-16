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
        public DbSet<Role> Roles { get; set; }

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
            

            // Seed roles
            var adminRole = new Role { Id = Guid.NewGuid(), AccessLevel = AccessLevel.Admin };
            var memberRole = new Role { Id = Guid.NewGuid(), AccessLevel = AccessLevel.Member };
            modelBuilder.Entity<Role>().HasData(adminRole, memberRole);

            // Create a hasher to hash the password before seeding the users
            var hasher = new PasswordHasher<User>();

            // Seed the admin user
            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                UserName = "admin@example.com",
                Email = "admin@example.com",
                NormalizedUserName = "ADMIN@EXAMPLE.COM",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                RoleId = adminRole.Id
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123!");

            modelBuilder.Entity<User>().HasData(adminUser);

            // Seed the member user
            var memberUser = new User
            {
                Id = Guid.NewGuid(),
                UserName = "member@example.com",
                Email = "member@example.com",
                NormalizedUserName = "MEMBER@EXAMPLE.COM",
                NormalizedEmail = "MEMBER@EXAMPLE.COM",
                EmailConfirmed = true,
                RoleId = memberRole.Id
            };
            memberUser.PasswordHash = hasher.HashPassword(memberUser, "Member123!");

            modelBuilder.Entity<User>().HasData(memberUser);
        }
    }
}