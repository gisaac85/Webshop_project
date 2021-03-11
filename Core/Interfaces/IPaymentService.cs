using System.Threading.Tasks;
using Core.Entities.BasketModels;
using Core.Entities.OrderModels;

namespace Core.Interfaces
{
    public interface IPaymentService
    {
         Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId);
         Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId);
         Task<Order> UpdateOrderPaymentFailed(string paymentIntentId);
    }
}