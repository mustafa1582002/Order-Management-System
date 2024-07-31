using Microsoft.AspNetCore.Identity;
using OrderManagement.Core;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;
using OrderManagement.Core.Services;
using OrderManagement.Core.Specifications.OrderSpecifications;
using OrderManagement.Core.Specifications.OrderItemSpecifications;
using OrderManagement.Service.EmailService;
using Order = OrderManagement.Core.Entities.Order;
using Product = OrderManagement.Core.Entities.Product;
using Customer = OrderManagement.Core.Entities.Identity.Customer;

namespace OrderManagement.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPaymentService _paymentService;
        private readonly IEmailService _emailService;

        public OrderService(IUnitOfWork unitOfWork
            ,UserManager<IdentityUser> userManager
            ,IPaymentService paymentService
            ,IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _paymentService = paymentService;
            _emailService = emailService;
        }

        //public async Task AssigningOrderIdToOrderItems(int OrderId)
        //{
        //    ///Assigning each orderitem to orderId
        //    var Spec = new OrderSpecification(OrderId);
        //    var assigningOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecificationAsync(Spec);
        //    if (assigningOrder.OrderItems?.Count > 0)
        //    {
        //        //assigningOrder.Id = OrderId;
        //        foreach (var item in assigningOrder.OrderItems)
        //            item.OrderId = assigningOrder.Id;
        //        //assigningOrder.OrderItems = OrderItems;
        //    }
        //    _unitOfWork.Repository<Order>().Update(assigningOrder);
        //    //////////////////
        //    var Result = await _unitOfWork.CompleteAsync();
        //    //if (Result <= 0)
        //    //    return null;
        //}
        public async Task GenerateInvoice(Order order)
        {

            ////////////////////
            Inovice invoice = new Inovice(order.Id, order, order.TotalAmount);
            await _unitOfWork.Repository<Inovice>().AddAsync(invoice);
            var Result = await _unitOfWork.CompleteAsync();
            //if (Result <= 0)
            //    return null;
            //throw new NotImplementedException();
        }

        public async Task<Order?> CreateAsync(string CustomerId, OrderToSaveDto order)
        {
            var OrderItems = new List<OrderItem>();
            if (order.OrderItems?.Count > 0)
            {
                foreach (var item in order.OrderItems)
                {
                    var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);
                    if (Product == null) return null;
                    if (item.Quantity>Product.Stock) return null;
                    var orderItem = new OrderItem();
                    orderItem.ProductId = Product.Id;
                    orderItem.Quantity = item.Quantity;
                    orderItem.UnitPrice = Product.Price;
                    orderItem.OrderId = null;
                    if (Product.Price > 100) orderItem.Discount = 0.5M;
                    else if (Product.Price > 200) orderItem.Discount = 1M;
                    else orderItem.Discount = 0;

                    OrderItems.Add(orderItem);
                    Product.Stock-=item.Quantity;   
                    _unitOfWork.Repository<Product>().Update(Product);
                }
            }
            var TotalAfterDiscount = OrderItems.Sum(Item => Item.UnitPrice * Item.Quantity-((decimal)Item.Discount * Item.UnitPrice));
            var Total = OrderItems.Sum(Item => Item.UnitPrice * Item.Quantity );

            //Create Order
            var Spec = new OrderWithPaymentIntentSpecification(order.PaymentIntentId);
            var ExOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecificationAsync(Spec);
            if(ExOrder != null)
            {
                _unitOfWork.Repository<Order>().Delete(ExOrder);
                var orderWithPaymentDto = new OrderWithPaymentDto(ExOrder.Id);
                await _paymentService.CreateOrUpdatePaymentIntent(orderWithPaymentDto);
            }

            var orderToSave = new Order(CustomerId,TotalAfterDiscount,OrderItems,order.PaymentIntentId,order.PaymentMethod );

            await _unitOfWork.Repository<Order>().AddAsync(orderToSave);
           

            var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0)
                return null;

            return orderToSave;
        }

        public async Task<IReadOnlyList<Order>> GetAllOrdersAsync()
        {
            var specs = new OrderSpecification();
            var order = await _unitOfWork.Repository<Order>().GetAllWithSpecificationAsync(specs);
            return order;
        }
        public async Task<Order> GetOrderById(int orderId)
        {
            var specs = new OrderSpecification(orderId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecificationAsync(specs);
            return (order);
        }

        public async Task<IReadOnlyList<Order>> GetOrders(string CustomerId)
        {
            var specs = new OrderSpecification(CustomerId);
            var order = await _unitOfWork.Repository<Order>().GetAllWithSpecificationAsync(specs);
            return order;
        }

        public async Task<bool> UpdateOrderStatus(int orderId, OrderStatus orderPaymentStatus, string BuyerEmail)
        {
            var spec = new OrderSpecification(orderId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecificationAsync(spec);
            if (order == null) return false;
            // todo : orderPaymentStatus
            order.Status = orderPaymentStatus;
            EmailSetting emailSetting = new EmailSetting()
            {
                Body = $"{orderPaymentStatus.ToString()}",
                TO = BuyerEmail,
                Title = "Orders From OrderMangmentSystem"
            };
            _emailService.SendEmail(emailSetting);
            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.CompleteAsync();
            return true;

        }
    }
}
