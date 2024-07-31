using OrderManagement.Core.Entities;

namespace OrderManagement.Core.Specifications.OrderItemSpecifications
{
    internal class OrderItemSpecification:BaseSpecification<OrderItem>
    {
        public OrderItemSpecification(int orderId) : base(O => O.OrderId == orderId)
        {
            Includes.Add(O => O.Product);
        }
    }
}
