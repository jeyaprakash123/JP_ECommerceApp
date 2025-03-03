using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace EcommerceApp.Order.Api.Domain.Model
{
    public class Orders
    {
        [Key]

        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }
    }
}
