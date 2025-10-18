using AutoMapper;
using common.MongoDB.Interface;
using MediatR;
using Order.Service.Entities;
using Order.Service.Features.Orders.Dtos;
using Order.Service.Features.Orders.Queries;


namespace Order.Service.Features.Orders.Handlers
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderSuccessAndErrorResponseDto<OrderHeaderResponseDto>>
    {
        private readonly IMongoRepository<OrderItem> _orderHeaderRepo;
        private readonly IMongoRepository<OrderDetails> _orderDetailsRepo;
        private readonly IMapper _mapper;
        public GetOrderByIdHandler(IMongoRepository<OrderItem> orderHeaderRepo, IMapper mapper, IMongoRepository<OrderDetails> orderDetailsRepo)
        {
            _orderHeaderRepo = orderHeaderRepo;
            _mapper = mapper;
            _orderDetailsRepo = orderDetailsRepo;
        }
        public async Task<OrderSuccessAndErrorResponseDto<OrderHeaderResponseDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            // Get order header
            var order = await _orderHeaderRepo.GetAsync(request.Id);
            if (order == null)
                return new OrderSuccessAndErrorResponseDto<OrderHeaderResponseDto>
                {
                    StatusCode = 400,
                    Message = "Order not found",
                    Data = null

                };

            var orderDetails = await _orderDetailsRepo.GetAllAsync(d => d.OrderItemId == order.Id);
            order.OrderDetails = orderDetails.ToList();
          
            var response =  _mapper.Map<OrderHeaderResponseDto>(order);

            return new OrderSuccessAndErrorResponseDto<OrderHeaderResponseDto>
            { 
                Data = response,
                Message = "Order Generated Successfully",
                StatusCode= 200
            };

        }
    }
}
