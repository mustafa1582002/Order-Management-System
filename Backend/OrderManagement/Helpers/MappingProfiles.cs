using AutoMapper;
using OrderManagement.Api.Dtos;
using OrderManagement.Core.Entities;

namespace OrderManagement.Api.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>();
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductName, O => O.MapFrom(d => d.Product.Name))
                .ForMember(d => d.ProductId, O => O.MapFrom(d => d.ProductId));

            CreateMap<Order, OrderToReturnDto>();
            CreateMap<Inovice, InvoiceDto>();

            
        }

    }
}
