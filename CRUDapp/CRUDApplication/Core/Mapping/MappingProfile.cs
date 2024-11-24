using AutoMapper;
using CRUDApplication.Entities;
using CRUDApplication.Entities.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CRUDApplication.Core.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Customer -> CustomerDto
            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.orders, opt => opt.MapFrom(src => src.orders));

            // CustomerDto -> Customer
            CreateMap<CustomerDto, Customer>();

            // Order -> OrderDto
            CreateMap<Order, OrderDto>();

            // OrderDto -> Order
            CreateMap<OrderDto, Order>();
        }
    }
}
