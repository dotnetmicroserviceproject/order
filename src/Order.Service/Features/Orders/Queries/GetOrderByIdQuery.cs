using MediatR;
using Order.Service.Features.Orders.Dtos;

namespace Order.Service.Features.Orders.Queries
{
    public class GetOrderByIdQuery : IRequest<OrderSuccessAndErrorResponseDto<OrderHeaderResponseDto>>
    {
        public Guid Id { get; set; }
        public GetOrderByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
