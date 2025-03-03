using System;
using ECommerceApp.Auth.Api.Domain.Model;
using ECommerceApp.Auth.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Polly.Retry;
using Polly;

namespace ECommerceApp.Auth.Api.Infrastructure.DataAccess
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Login> Logins { get; set; }
        public DbSet<Roles> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Login>().Property(l => l.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Login>().HasData(
                new Login
                {
                    Id = 121,
                    UserId = "U123",
                    Username = "Admin",
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin@12"),
                    Age = 21,
                    City = "Madurai",
                    Pincode = 1234,
                    RoleId = 1
                }
                );
            modelBuilder.Entity<Roles>().HasData(
                new Roles { RoleId = 1, RoleName = "Admin" },
                new Roles { RoleId = 2, RoleName = "User" }
            );
        }

        public static async Task EnsureDbCreatedAsync(IServiceProvider services, string? initialImportDataDir)
        {
            using var scope = services.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            var pipeline = new ResiliencePipelineBuilder().AddRetry(new RetryStrategyOptions { Delay = TimeSpan.FromSeconds(3) }).Build();
            var createdDb = await pipeline.ExecuteAsync(async (CancellationToken ct) =>
                await dbContext.Database.EnsureCreatedAsync(ct));
        }
    }
}
