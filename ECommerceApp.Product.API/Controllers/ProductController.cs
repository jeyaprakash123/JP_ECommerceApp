using System.Runtime.CompilerServices;
using Confluent.Kafka;
using ECommerceApp.Product.Api.Application.Interface;
using ECommerceApp.Product.Api.Application.Services;
using ECommerceApp.Product.Api.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using static Confluent.Kafka.ConfigPropertyNames;
using System.Text.Json;


namespace ECommerceApp.Product.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IDistributedCache _cache;
        public ProductController(IProductService productService, IDistributedCache cache)
        {
            _productService = productService;
            _cache = cache;
        }

        [HttpGet("/get-all-product")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAllProduct()
        {
            var cacheKey = "product_list";
            var cacheRes = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cacheRes))
            {
                return Ok(JsonSerializer.Deserialize<List<Products>>(cacheRes));
            }
            var result = await _productService.SelectProductAsync();

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });


            return Ok(result);
        }

        [HttpGet("/get-product")]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> GetProductByName([FromQuery] string productName)
        {
            var cacheResult = await _cache.GetStringAsync($"product_{productName}");
            if (!string.IsNullOrEmpty(cacheResult))
            {
                return Ok(cacheResult);
            }

            var result = await _productService.SelectProductByNameAsync(productName);
            await _cache.SetStringAsync($"product_{productName}", JsonSerializer.Serialize(result), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

            return Ok(result);
        }

        [HttpPost("/add-product")]
        // [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductRequest product)
        {
            await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(CreateProduct), product);
        }

        [HttpPost("/select")]
        public async Task<IActionResult> SelectProduct(string name, int quantity)
        {
            var result = await _productService.PublishProductEventAsync(name, quantity);
            
            return Ok(new { message = "Product selection event published to Kafka" });
        }

        [HttpPut]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateProductById([FromBody]Products product)
        {
            var res = await _productService.UpdateProductAsync(product);
            return Ok("Successfully product deatils updated");
        }

        [HttpDelete]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteProductById(string productname)
        {
            await _productService.DeleteProductAsync(productname);
            return NoContent();
        }
    }
}
