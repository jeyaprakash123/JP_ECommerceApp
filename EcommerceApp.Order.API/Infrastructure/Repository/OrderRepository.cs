using EcommerceApp.Order.Api.Domain.Interface;
using EcommerceApp.Order.Api.Domain.Model;
using EcommerceApp.Order.Api.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Order.Api.Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _orderDbContext;
        public OrderRepository(OrderDbContext orderDbContext) 
        {
            _orderDbContext = orderDbContext;
        }

        public async Task<IEnumerable<Orders>> GetAllOrderAsync(int? id)
        {
            if(id is null) return await _orderDbContext.Orderss.AsQueryable().ToListAsync();

            return await _orderDbContext.Orderss.Where(x=>x.OrderId == id).ToListAsync();
            
        }

        public async Task<Orders> PlaceOrder(Orders orders)
        {
            await _orderDbContext.Orderss.AddAsync(orders);
            await _orderDbContext.SaveChangesAsync();

            return orders;
        }

        public async Task<Orders> UpdateOrderAsync(Orders order)
        {
            var res = await _orderDbContext.Orderss.SingleOrDefaultAsync(x=>x.OrderId==order.OrderId);
            res.Price=order.Price;
            res.OrderId=order.OrderId;
            res.ProductId=order.ProductId;
            res.Quantity=order.Quantity;
             _orderDbContext.Orderss.Update(res);
            await _orderDbContext.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeletOrderAsync(int orderId)
        {
            var res = await _orderDbContext.Orderss.SingleOrDefaultAsync(x=>x.OrderId == orderId);
            _orderDbContext.Orderss.Remove(res);
            await _orderDbContext.SaveChangesAsync();
            return true;
        }
    }
}
