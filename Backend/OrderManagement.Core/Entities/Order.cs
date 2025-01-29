using OrderManagement.Core.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.Core.Entities
{
    public class Order:BaseEntity
    {
        public Order()
        {

        }
        public Order(string customerId, decimal totalAmount, ICollection<OrderItem> orderItems, string paymentIntentId,string paymentMethod)
        {
            CustomerId = customerId;
            TotalAmount = totalAmount;
            OrderItems = orderItems;
            PaymentIntentId = paymentIntentId;
            PaymentMethod = paymentMethod;
        }
        public Order(string customerId, string paymentIntentId)
        {
            CustomerId = customerId;
            PaymentIntentId = paymentIntentId;
        }
        public DateTimeOffset OrderDate { get; set; }=DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string PaymentMethod { get; set; }
        public string PaymentIntentId { get; set; }=string.Empty;
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }=new HashSet<OrderItem>();
        public string ClientSecret { get; set; }
        public string? CustomerId { get; set; } 
        //public Customer Customer { get; set; } 

    }
}