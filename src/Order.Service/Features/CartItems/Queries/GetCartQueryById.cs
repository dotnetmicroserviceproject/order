using MediatR;
using Order.Service.Features.CartItems.Dtos;

namespace Order.Service.Features.CartItems.Queries
{
    public class GetCartQueryById : IRequest<IEnumerable<CartDto>>
    {
        
    }
}
