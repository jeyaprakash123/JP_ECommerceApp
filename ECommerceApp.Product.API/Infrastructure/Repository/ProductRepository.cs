using System.Runtime.CompilerServices;
using ECommerceApp.Product.Api.Domain.Interface;
using ECommerceApp.Product.Api.Domain.Model;
using ECommerceApp.Product.Api.Infrastructure.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Product.Api.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;
        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Products>> GetProductsAsync(string? productname)
        {
            if (string.IsNullOrWhiteSpace(productname))
            {
                return await _context.productss.Include(i=>i.Catagory).AsQueryable().ToListAsync();
            }
            return await _context.productss.Include(i => i.Catagory).Where(p => p.ProductName == productname).ToListAsync();
        }
        public async Task<Products> CreateProductsAsync(Products product)
        {
            if (product is null)
            {
                return null;
            }
            await _context.productss.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;

        }
        public async Task<Products> UpdateProductsAsync(Products products)
        {
            var res = await _context.productss.SingleOrDefaultAsync(x => x.ProductId == products.ProductId);
            if (res != null)
            {
                res.ProductName = products.ProductName;
                res.ManufactureDate = products.ManufactureDate;
                res.ExpiryDate = products.ExpiryDate;
                res.Price = products.Price;

                _context.productss.Update(res);
                await _context.SaveChangesAsync();

                return products;


            }
            return new Products();
        }

        public async Task<bool> DeleteProductsAsync(string productName)
        {
            var res = await _context.productss.SingleOrDefaultAsync(y => y.ProductName == productName);

            _context.productss.Remove(res);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
