using ECommerceApp.Product.Api.Domain.Model;

namespace ECommerceApp.Product.Api.Application.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<Products>> SelectProductAsync();
        Task<IEnumerable<Products>> SelectProductByNameAsync(string name);

        Task<Products> AddProductAsync(ProductRequest product);

        Task<ProductSelectEvent> PublishProductEventAsync(string name, int quantity);

        Task<bool> DeleteProductAsync(string productname);

        Task<Products> UpdateProductAsync(Products product);
    }
}
