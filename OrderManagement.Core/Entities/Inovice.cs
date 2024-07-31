namespace OrderManagement.Core.Entities
{
    public class Inovice:BaseEntity
    {
        public Inovice(int orderId, Order order, decimal totalAmount)
        {
            OrderId = orderId;
            Order = order;
            TotalAmount = totalAmount;
            InvoiceDate = DateTime.Now;
        }
        public Inovice()
        {
            
        }

        public int OrderId { get; set; }
        public Order Order { get; set; }
        public DateTime InvoiceDate { get; set; }= DateTime.Now;
        public decimal TotalAmount { get; set; }
    }
}
