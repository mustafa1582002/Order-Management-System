using OrderManagement.Core.Entities;

namespace OrderManagement.Api.Dtos
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public float Discount { get; set; }
    }
}