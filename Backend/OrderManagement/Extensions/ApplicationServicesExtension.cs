using Microsoft.AspNetCore.Mvc;
using OrderManagement.Api.Errors;
using OrderManagement.Api.Helpers;
using OrderManagement.Core;
using OrderManagement.Core.Interfaces;
using OrderManagement.Core.Services;
using OrderManagement.Repository;
using OrderManagement.Repository.Repositories;
using OrderManagement.Service;
using OrderManagement.Service.EmailService;

namespace OrderManagement.Api.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddScoped(typeof(IEmailService), typeof(EmailService));
            Services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            Services.AddScoped(typeof(IOrderService), typeof(OrderService));
            Services.AddScoped(typeof(IGenericRepositories<>), typeof(GenericRepositories<>));
            Services.AddAutoMapper(typeof(MappingProfiles));
            Services.Configure<ApiBehaviorOptions>(Options =>
            {
                Options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                                          .SelectMany(p => p.Value.Errors)
                                                          .Select(E => E.ErrorMessage)
                                                          .ToArray();
                    var validiationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(validiationErrorResponse);
                };
            });
            return Services;
        }
    }
}
