using EcommerceApp.Order.Api.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Order.Api.Application.Interface
{
    public interface IOrderService
    {
        Task<IEnumerable<Orders>> GetAllOrderAsync();

        Task<IEnumerable<Orders>> GetOrderByIdAsync(int orderId);

        Task<Orders> PlaceOrder(Orders order);

        Task<PlaceOrderEvent> PlaceOrder();

        Task<Orders> UpdateOrderAsync(Orders order);

        Task<bool>  DeleteOrderAsync(int orderId);
    }
}
