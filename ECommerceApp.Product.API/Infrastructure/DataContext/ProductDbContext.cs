using ECommerceApp.Product.Api.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Polly.Retry;
using Polly;

namespace ECommerceApp.Product.Api.Infrastructure.DataContext
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options):base(options) { }


        public DbSet<Products> productss { get; set; }

        public DbSet<Catagory> catagories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Products>().Property(l => l.ProductId).ValueGeneratedOnAdd();
            modelBuilder.Entity<Catagory>().Property(l => l.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Catagory>().HasData(
                new Catagory
                {
                    Id = 1,
                    CatagoryName = "Fruits"
                },
                new Catagory
                {
                    Id = 2,
                    CatagoryName = "Vegetables"
                }
                );
            modelBuilder.Entity<Products>().HasData(
                new Products
                {
                    ProductId = 1,
                    ProductName = "Apple",
                    Price = 150,
                    Id = 1,
                    ManufactureDate = new DateOnly(2025, 02, 21),
                    ExpiryDate = new DateOnly(2025, 05, 21)
                },
                new Products
                {
                    ProductId = 2,
                    ProductName = "Brinjal",
                    Price = 42,
                    Id = 2,
                    ManufactureDate = new DateOnly(2025, 02, 19),
                    ExpiryDate = new DateOnly(2025, 02, 24)
                });
        }

        public static async Task EnsureDbCreatedAsync(IServiceProvider services, string? initialImportDataDir)
        {
            using var scope = services.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

            
            var pipeline = new ResiliencePipelineBuilder().AddRetry(new RetryStrategyOptions { Delay = TimeSpan.FromSeconds(3) }).Build();
            var createdDb = await pipeline.ExecuteAsync(async (CancellationToken ct) =>
                await dbContext.Database.EnsureCreatedAsync(ct));

        }
    }
}
