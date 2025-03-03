using Confluent.Kafka;
using EcommerceApp.Order.Api.Application.Interface;
using EcommerceApp.Order.Api.Domain.Interface;
using EcommerceApp.Order.Api.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EcommerceApp.Order.Api.Application.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IConsumer<string,string> _consumer;
        private readonly IProducer<string,string> _producer;
        public OrderService(IOrderRepository orderRepository, IProducer<string, string> producer, IConsumer<string, string> consumer)
        {
            _orderRepository = orderRepository;
            _producer = producer;
            _consumer = consumer;
        }

        public async Task<IEnumerable<Orders>> GetAllOrderAsync()
        {
            return await _orderRepository.GetAllOrderAsync(null);
        }
        public async Task<IEnumerable<Orders>> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository.GetAllOrderAsync(orderId);
        }

        public async Task<Orders> PlaceOrder(Orders order)
        {
            return await _orderRepository.PlaceOrder(order);
        }

        public async Task<PlaceOrderEvent> PlaceOrder()
        {
            _consumer.Subscribe("Product-Selected");
            var consumeResult = _consumer.Consume();
            var message = JsonConvert.DeserializeObject<ProductSelectedEvent>(consumeResult.Message.Value);
            if(message is null)
            {
                throw new Exception();
            }
            PlaceOrderEvent placeOrder = new PlaceOrderEvent
            {
                OrderId = new Guid().ToString().Substring(1, 2),
                ProductId = message.ProductId,
                CreatedDate = DateTime.Now,
                Quantity= message.Quantity,
                price=message.price * message.Quantity
            };

            var producerMessage = new Message<string, string>
            {
                Key = placeOrder.OrderId,
                Value = JsonConvert.SerializeObject(placeOrder)
            };
            await _producer.ProduceAsync("Place-Order-Topic", producerMessage);
            return placeOrder;
        }

        public async Task<Orders> UpdateOrderAsync(Orders order)
        {
           return await _orderRepository.UpdateOrderAsync(order);
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            return await _orderRepository.DeletOrderAsync(orderId);
        }
    }
}
