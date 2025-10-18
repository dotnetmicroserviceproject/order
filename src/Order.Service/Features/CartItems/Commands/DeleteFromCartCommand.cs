using MediatR;
using Order.Service.Features.CartItems.Dtos;

namespace Order.Service.Features.CartItems.Commands
{
    public class DeleteFromCartCommand : IRequest<CartErrorAndSuccessResponseDto<CartResponseDto>>
    {
        public Guid CartId { get; set; }

        public DeleteFromCartCommand(Guid cartId)
        {
            CartId = cartId;
        }
    }
}
