using MediatR;
using Order.Service.Features.Orders.Dtos;

namespace Order.Service.Features.Orders.Commands
{
    public class CreateOrderCommand : IRequest<OrderSuccessAndErrorResponseDto<OrderHeaderResponseDto>>
    {
        public OrderHeaderCreateDto OrderHeaderCreateDto { get; set; }
        public CreateOrderCommand(OrderHeaderCreateDto orderHeaderCreateDto)
        {
            OrderHeaderCreateDto = orderHeaderCreateDto;
        }
    }
}
