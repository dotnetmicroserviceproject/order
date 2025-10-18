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
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, CartErrorAndSuccessResponseDto<CartResponseDto>>
    {
        private readonly IMongoRepository<CartItem> _cartRepo;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        private readonly IMongoRepository<ProductItem> _productRepo;
        private readonly IMongoRepository<UserItem> _userRepo;

        public AddToCartCommandHandler(IMongoRepository<CartItem> cartRepo, IPublishEndpoint publishEndpoint, IMapper mapper, IMongoRepository<ProductItem> productRepo, IMongoRepository<UserItem> userRepo)
        {
            _cartRepo = cartRepo;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
            _productRepo = productRepo;
            _userRepo = userRepo;
        }

        public async Task<CartErrorAndSuccessResponseDto<CartResponseDto>> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var userId = await _userRepo.GetAsync(x => x.Id == request.CartItemRequest.UserId);

            if (userId == null)
                return new CartErrorAndSuccessResponseDto<CartResponseDto>
                {
                    Message = "User not Found.",
                    StatusCode = 400,
                    Data = null
                };

            // 2️⃣ Fetch product details from Product Service
            var product = await _productRepo.GetAsync(x=> x.Id ==request.CartItemRequest.ProductId);
            if (product == null || product.StockQuantity < request.CartItemRequest.Quantity)
                return new CartErrorAndSuccessResponseDto<CartResponseDto>
                {
                    Message = "Product not available or insufficient stock..",
                    StatusCode = 404,
                    Data = null
                };

            // 3️⃣ Check if cart item already exists
            var cartItem = await _cartRepo
                .GetAsync(x => x.UserId == request.CartItemRequest.UserId && x.ProductItemId == request.CartItemRequest.ProductId);

            if (cartItem != null)
            {
                cartItem.Quantity += request.CartItemRequest.Quantity;
                await _cartRepo.UpdateAsync(cartItem);
            }
            else
            {
                 cartItem = new CartItem
                {
                    UserId = request.CartItemRequest.UserId,
                    ProductItemId = request.CartItemRequest.ProductId,
                    Quantity = request.CartItemRequest.Quantity,
                    CreatedDate = DateTime.UtcNow,
                };
                await _cartRepo.CreateAsync(cartItem);
            }

            await _publishEndpoint.Publish(new CartItemUpdated(
              cartItem.UserId,
              cartItem.ProductItemId,
              cartItem.Quantity
              
          ));
            var response =  _mapper.Map<CartResponseDto>(cartItem);

            return new CartErrorAndSuccessResponseDto<CartResponseDto>
            {
                Data = response,
                Message = "Cart Created Successfully",
                StatusCode = 201
            };
        }
    }
}
