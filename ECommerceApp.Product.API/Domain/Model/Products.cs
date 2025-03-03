using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.Product.Api.Domain.Model
{
    public class Products
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }

        public double Price { get; set; }

        public DateOnly ManufactureDate { get; set; }

        public DateOnly ExpiryDate { get; set; }

        [ForeignKey("Catagory")]
        public int Id {  get; set; }
        public virtual Catagory Catagory { get; set; }
    }
}
