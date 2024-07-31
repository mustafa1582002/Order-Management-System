using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Api.Errors;
using OrderManagement.Core.Interfaces;
using OrderManagement.Core.Services;
using Stripe;

namespace OrderManagement.Api.Controllers
{
    public class PaymentController : APIBaseController
    {
        private readonly IPaymentService _paymentService;
        const string endpointSecret = "whsec_e73f4a699fbc0833d478d6d689889042fc9fa01e4bfeadfef7b75f4d28275697";

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [Authorize]
        [ProducesResponseType(typeof(OrderWithPaymentDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [HttpPost]
        public async Task<ActionResult<OrderWithPaymentDto?>> CreateOrUpdate(OrderWithPaymentDto order)
        {
            var BasketOrder = await _paymentService.CreateOrUpdatePaymentIntent(order);
            if (BasketOrder == null) return BadRequest(new ApiResponse(400, "Basket wrong"));
            //Mapping
            return Ok(BasketOrder);
        }
        [HttpPost("webhock")]
        public async Task<IActionResult> StripeWebHock()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                var PaymentIntent = stripeEvent.Data.Object as PaymentIntent;
                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    await _paymentService.UpdatePaymentIntentToSucceedOrFailed(PaymentIntent.Id, false);
                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    await _paymentService.UpdatePaymentIntentToSucceedOrFailed(PaymentIntent.Id, true);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}
