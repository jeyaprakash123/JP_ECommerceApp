using Microsoft.EntityFrameworkCore;
using EcommerceApp.Order.Api.Domain.Model;
using Polly.Retry;
using Polly;

namespace EcommerceApp.Order.Api.Infrastructure.DataContext
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<Orders> Orderss { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Orders>().Property(l => l.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Orders>().HasData(
                new Orders
                {
                    Id = 101,
                    OrderId = 1,
                    ProductId = 1,
                    Price = 150,
                    Quantity = 2
                });
        }

        public static async Task EnsureDbCreatedAsync(IServiceProvider services, string? initialImportDataDir)
        {
            using var scope = services.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();


            var pipeline = new ResiliencePipelineBuilder().AddRetry(new RetryStrategyOptions { Delay = TimeSpan.FromSeconds(3) }).Build();
            var createdDb = await pipeline.ExecuteAsync(async (CancellationToken ct) =>
                await dbContext.Database.EnsureCreatedAsync(ct));

        }
    }
}
