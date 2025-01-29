using OrderManagement.Core.Interfaces;

namespace OrderManagement.Core.Entities
{
    public class OrderToSaveDto
    {
        public string CustomerEmail { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; } = new HashSet<OrderItemDto>();
        public string PaymentMethod { get; set; }
        public string PaymentIntentId { get; set; }
    }
}
