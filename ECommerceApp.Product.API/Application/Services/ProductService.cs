using Confluent.Kafka;
using ECommerceApp.Product.Api.Application.Interface;
using ECommerceApp.Product.Api.Domain.Interface;
using ECommerceApp.Product.Api.Domain.Model;
using Newtonsoft.Json;

namespace ECommerceApp.Product.Api.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProducer<string, string> _producer;
        public ProductService(IProductRepository productRepository, IProducer<string, string> producer)
        {
            _productRepository = productRepository;
            _producer = producer;
        }

        public async Task<IEnumerable<Products>> SelectProductAsync()
        {
            return await _productRepository.GetProductsAsync(null);
        }

        public async Task<IEnumerable<Products>> SelectProductByNameAsync(string name)
        {
            return await _productRepository.GetProductsAsync(name);
        }
        public async Task<Products> AddProductAsync(ProductRequest product)
        {
            var newProd = new Products
            {
                ProductName = product.ProductName,
                Price = product.Price,
                Id = product.CatagoryId,
                ManufactureDate = product.ManufactureDate,
                ExpiryDate = product.ExpiryDate
            };
            return await _productRepository.CreateProductsAsync(newProd);
        }

        public async Task<ProductSelectEvent> PublishProductEventAsync(string name, int quantity)
        {
            var _event = await _productRepository.GetProductsAsync(name);
            ProductSelectEvent productSelectEvent = new ProductSelectEvent
            {
                ProductId = _event.First().ProductId.ToString(),
                ProductName = _event.First().ProductName,
                price = _event.First().Price,
                Quantity = quantity

            };

            var message = new Message<string, string>
            {
                Key = productSelectEvent.ProductId,
                Value = JsonConvert.SerializeObject(productSelectEvent)
            };
            await _producer.ProduceAsync("Product-Selected", message);
            return productSelectEvent;
        }

        public async Task<Products> UpdateProductAsync(Products product)
        {
            return await _productRepository.UpdateProductsAsync(product);
        }

        public async Task<bool> DeleteProductAsync(string productname)
        {
            return await _productRepository.DeleteProductsAsync(productname);
        }
    }
}
