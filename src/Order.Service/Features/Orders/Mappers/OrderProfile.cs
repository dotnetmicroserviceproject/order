using Order.Service.Entities;
using Order.Service.Features.Orders.Dtos;


namespace Order.Service.Features.Orders.Mappers
{
    public class OrderProfile : AutoMapper.Profile
    {
        public OrderProfile()
        {
            // ✅ Mapping for creating an order
            CreateMap<OrderHeaderCreateDto, OrderItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Mongo/DB will generate this
                .ForMember(dest => dest.OrderDetails, opt => opt.Ignore()) // handled manually in handler
                .ForMember(dest => dest.OrderDate, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.TotalItems, opt => opt.Ignore())
                .ForMember(dest => dest.OrderTotal, opt => opt.Ignore());

            // ✅ Mapping for updating an order
            CreateMap<OrderHeaderUpdateDto, OrderItem>().ReverseMap();
            CreateMap<OrderDetails, OrderDetailDto>().ReverseMap();
            CreateMap<OrderItem, OrderHeaderResponseDto>()
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));
        }
    }
}
