using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Product.Api.Domain.Model
{
    public class ProductRequest
    {
        public string ProductName { get; set; }

        public double Price { get; set; }

        public DateOnly ManufactureDate { get; set; }

        public DateOnly ExpiryDate { get; set; }

        public int CatagoryId { get; set; }
    }
}
