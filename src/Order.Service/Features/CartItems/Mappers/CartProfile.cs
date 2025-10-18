using AutoMapper;
using Order.Service.Entities;
using Order.Service.Features.CartItems.Dtos;

namespace Order.Service.Features.CartItems.Mappers
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CartItem, CartResponseDto>().ReverseMap();
        }
    }
}
