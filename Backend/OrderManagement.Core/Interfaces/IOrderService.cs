using OrderManagement.Core.Entities;

namespace OrderManagement.Core.Interfaces
{
    public interface IOrderService
    {
        Task<Order?> CreateAsync(string CustomerId,OrderToSaveDto order);
        Task<Order> GetOrderById( int OrderId);
        Task<IReadOnlyList<Order>> GetOrders(string CustomerId);
        Task<IReadOnlyList<Order>> GetAllOrdersAsync();
        Task<bool> UpdateOrderStatus(int orderId, OrderStatus orderPaymentStatus, string BuyerEmail);
        //Task AssigningOrderIdToOrderItems(int OrderId);
        Task GenerateInvoice(Order order);
    }
}
