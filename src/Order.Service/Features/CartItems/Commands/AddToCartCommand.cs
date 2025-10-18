using MediatR;
using Order.Service.Features.CartItems.Dtos;

namespace Order.Service.Features.CartItems.Commands
{
    public class AddToCartCommand : IRequest<CartErrorAndSuccessResponseDto<CartResponseDto>>
    {
        public CartItemDto CartItemRequest { get; set; }

        public AddToCartCommand(CartItemDto cartItemRequest)
        {
            CartItemRequest = cartItemRequest;
        }
    };
  
}
