using EcommerceApp.Order.Api.Domain.Model;

namespace EcommerceApp.Order.Api.Domain.Interface
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Orders>> GetAllOrderAsync(int? orderId);

        Task<Orders> PlaceOrder(Orders orders);

        Task<Orders> UpdateOrderAsync(Orders orders);

        Task<bool> DeletOrderAsync(int orderId);
    }
}
