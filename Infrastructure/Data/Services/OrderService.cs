using Core.Entities.OrderModels;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Services
{
    public class OrderService : IOrderService
    {
        private readonly IProductRepository _productRepo;
        private readonly IBasketRepository _basketRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IDeliveryRepository _deliveryRepo;       

        public OrderService(IProductRepository productRepo, IBasketRepository basketRepo, IOrderRepository orderRepo, IDeliveryRepository deliveryRepo)
        {
            _productRepo = productRepo;
            _basketRepo = basketRepo;
            _orderRepo = orderRepo;
            _deliveryRepo = deliveryRepo;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            var basket = await _basketRepo.GetBasketAsync(basketId);
            var items = new List<OrderItem>();
            foreach(var item in basket.Items)
            {
                var productItem = await _productRepo.GetProductByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name,productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }
            var deliveryMethod = await _deliveryRepo.GetDeliveryMethod(deliveryMethodId);
            var subTotal = items.Sum(item => item.Price * item.Quantity);
            var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subTotal,"");
            await _orderRepo.CreateOrder(order);
            await _basketRepo.DeleteBasketAsync(basketId);
            return order;
        }

        public async Task<DeliveryMethod> GetDeliveryMethod(int id)
        {
            var model = _orderRepo.GetDeliveryMethod(id);
            return await model;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var model = _deliveryRepo.GetDeliveryMethodsAsync();
            return await model;
        }

        public async Task<Order> GetOrderByIdAsync(int id, string email)
        {
            var model = _orderRepo.GetOrder(id,email);
            return await model;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var model = _orderRepo.GetOrdersForUser(buyerEmail);
            return await model;
        }

        public async Task<Order> UpdateOrder(Order input,string basketId)
        {
            var basket = await _basketRepo.GetBasketAsync(basketId);
            var model = _orderRepo.EditOrder(input);
            await _basketRepo.DeleteBasketAsync(basketId);
            return await model;
        }
    }
}
