using OrderManagement.Core.Entities;

namespace OrderManagement.Api.Dtos
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; } = new HashSet<OrderItemDto>();
        
    }
}
