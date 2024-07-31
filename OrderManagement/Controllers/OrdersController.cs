using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Api.Dtos;
using OrderManagement.Api.Errors;
using OrderManagement.Core;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;
using OrderManagement.Core.Specifications.OrderSpecifications;
using System.Security.Claims;

namespace OrderManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService
            ,UserManager<IdentityUser> userManager
            ,IMapper mapper
            ,IUnitOfWork unitOfWork
            , ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        [Authorize]
        [HttpPost("CreateOrder")]
        [ProducesResponseType(typeof(Order), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<Order>>CreateOrder(OrderToSaveDto order)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Customer=await _userManager.FindByEmailAsync(Email);
            //Customer.Id == Buyer Emial
            var Order = await _orderService.CreateAsync(Customer.Id,order);
            if (Order is null) return BadRequest(new ApiResponse(400, "There is a problem with your Order"));
            //await _orderService.AssigningOrderIdToOrderItems(Order.Id);
            await _orderService.GenerateInvoice(Order);
            return Ok(Order);
        }
        [Authorize]
        [HttpGet("GetOrdersForSpecificUser")]
        [ProducesResponseType(typeof(IReadOnlyList<Order>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForSpecificUser()
        {
            ///it should be Customer Id
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Customer = await _userManager.FindByEmailAsync(BuyerEmail);
            var Spec = new OrderSpecification(Customer.Id);
            var Orders = await _unitOfWork.Repository<Order>().GetAllWithSpecificationAsync(Spec);
            if (Orders is null) return NotFound(new ApiResponse(404, "There is no Order"));
            var MappedOrders = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(Orders);
            
            return Ok(MappedOrders);
        }




        //[Authorize(Roles = "Admin")]
        //[HttpGet("GetAllOrders")]
        //[ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), 200)]
        //public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetAllOrders()
        //{
        //    var orders = await _orderService.GetAllOrdersAsync();
        //    var mappedOrder = _mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);
        //    return Ok(mappedOrder);
        //}


        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Order), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null)
                return BadRequest(new ApiResponse(404, "Not Found"));
            var mappedOrder = _mapper.Map<OrderToReturnDto>(order);
            return Ok(mappedOrder);
        }



        [HttpPut("{orderId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<bool>> UpdateOrderStatus(int orderId, OrderStatus orderPaymentStatus)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var UpdateSuccess = await _orderService.UpdateOrderStatus(orderId, orderPaymentStatus,BuyerEmail);
            return UpdateSuccess ? Ok(UpdateSuccess) : BadRequest(new ApiResponse(400, "Update failed. Reason: Insufficient permissions"));
        }
    }
}
