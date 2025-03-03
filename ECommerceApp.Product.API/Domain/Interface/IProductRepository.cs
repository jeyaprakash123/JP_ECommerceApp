using ECommerceApp.Product.Api.Domain.Model;

namespace ECommerceApp.Product.Api.Domain.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<Products>> GetProductsAsync(string? productname);
        Task<Products> CreateProductsAsync(Products product);
        Task<bool> DeleteProductsAsync(string productName);
        Task<Products> UpdateProductsAsync(Products products);
    }
}
