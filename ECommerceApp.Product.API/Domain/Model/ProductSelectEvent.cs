namespace ECommerceApp.Product.Api.Domain.Model
{
    public class ProductSelectEvent
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public double price { get; set; }

        public int Quantity {  get; set; }
    }
}
