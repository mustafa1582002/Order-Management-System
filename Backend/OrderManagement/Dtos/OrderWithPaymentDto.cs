using OrderManagement.Api.Dtos;

namespace OrderManagement.Core.Entities
{
    public class OrderWithPaymentDto
    {
        public int Id { get; set; }
        public IReadOnlyList<OrderItemDto> OrderItems { get; set; }
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
        public OrderWithPaymentDto(int id)
        {
            Id = id;
        }

    }
}
