using OrderManagement.Core.Entities;

namespace OrderManagement.Core.Specifications.OrderSpecifications
{
    public class OrderWithPaymentIntentSpecification:BaseSpecification<Order>
    {
        public OrderWithPaymentIntentSpecification(string PaymentIntentId) : base(O => O.PaymentIntentId == PaymentIntentId)
        {
        }
    }
}
