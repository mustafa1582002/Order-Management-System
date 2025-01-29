namespace OrderManagement.Core.Interfaces
{
    public class OrderWithPaymentDto
    {
        public int Id { get; set; }
        public IReadOnlyList<OrderItemDto> OrderItems { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public OrderWithPaymentDto(int id)
        {
            Id = id;
        }

    }
}
