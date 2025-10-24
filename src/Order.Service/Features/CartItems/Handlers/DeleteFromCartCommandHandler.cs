using AutoMapper;
using common.MongoDB.Interface;
using MassTransit;
using MediatR;
using Order.Contracts;
using Order.Service.Entities;
using Order.Service.Features.CartItems.Commands;
using Order.Service.Features.CartItems.Dtos;

namespace Order.Service.Features.CartItems.Handlers
{
    public class DeleteFromCartCommandHandler : IRequestHandler<DeleteFromCartCommand, CartErrorAndSuccessResponseDto<CartResponseDto>>
    {
        private readonly IMongoRepository<CartItem> _cartRepo;
        private readonly IPublishEndpoint _publishEndpoint;


        public DeleteFromCartCommandHandler(IMongoRepository<CartItem> cartRepo,IMongoRepository<ProductItem> productRepo, IPublishEndpoint publishEndpoint)
        {
            _cartRepo = cartRepo;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<CartErrorAndSuccessResponseDto<CartResponseDto>> Handle(DeleteFromCartCommand request, CancellationToken cancellationToken)
        {

            var cartItem = await _cartRepo.GetAsync(x => x.Id == request.CartId);
            if (cartItem == null)
                return new CartErrorAndSuccessResponseDto<CartResponseDto>
                {
                    StatusCode = 400 ,
                    Message = "Cart Not Found",
                    Data = null
                };

            await _cartRepo.DeleteAsync(cartItem);
            await _publishEndpoint.Publish(new CartItemDeleted(cartItem.Id));

            return new CartErrorAndSuccessResponseDto<CartResponseDto>
            {
                Data = null,
                Message = "Product Deleted Successfully from Cart",
                StatusCode = 200
            };
        }
    }
}
