using OrderManagement.Core.Entities;

namespace OrderManagement.Core.Specifications.OrderSpecifications
{
    public class OrderSpecification : BaseSpecification<Order>
    {
        
        public OrderSpecification(string? customerId = null) :
            base(order =>
                string.IsNullOrEmpty(customerId) || order.CustomerId == customerId)
        {
            Includes.Add(order => order.OrderItems);
        }
        public OrderSpecification(int id) :
           base(order => order.Id == id)
        {
            Includes.Add(order => order.OrderItems);
            //Includes.Add(order => order.Customer);
        }

        ///////// for Assigning OrderItems
        public OrderSpecification(string CustomerId, bool flag) : base(O => O.CustomerId == CustomerId && O.OrderItems.FirstOrDefault().OrderId == 0)
        {
            Includes.Add(O => O.OrderItems);
        }
        //public OrderSpecification(string CustomerId, int orderId) : base(O => O.CustomerId == CustomerId && O.Id== orderId)
        //{
        //    Includes.Add(O => O.OrderItems);
        //}
    }

}
