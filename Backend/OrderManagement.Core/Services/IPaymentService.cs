using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;

namespace OrderManagement.Core.Services
{
    public interface IPaymentService
    {
        Task<OrderWithPaymentDto?> CreateOrUpdatePaymentIntent(OrderWithPaymentDto order);
        Task<Order> UpdatePaymentIntentToSucceedOrFailed(string PaymentIntentId,bool flag);
    }
}
