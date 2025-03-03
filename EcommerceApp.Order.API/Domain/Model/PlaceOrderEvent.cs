namespace EcommerceApp.Order.Api.Domain.Model
{
    public class PlaceOrderEvent
    {
        public string OrderId { get; set; }

        public string ProductId { get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedDate { get; set; }

        public double price { get; set; }
    }
}
