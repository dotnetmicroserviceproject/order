using AutoMapper;
using common.MongoDB.Interface;
using MediatR;
using Order.Service.Entities;
using Order.Service.Extensions;
using Order.Service.Features.CartItems.Dtos;
using Order.Service.Features.CartItems.Queries;

namespace Order.Service.Features.CartItems.Handlers
{
    public class GetCartQueryHandler : IRequestHandler<GetCartQueryById, IEnumerable<CartDto>>
    {
        private readonly IMongoRepository<CartItem> _cartRepo;
        private readonly IMongoRepository<ProductItem> _productRepo;
        public GetCartQueryHandler(IMongoRepository<CartItem> cartRepo, IMongoRepository<ProductItem> productRepo)
        {
            _cartRepo = cartRepo;
            _productRepo = productRepo;
        }
        public async Task<IEnumerable<CartDto>> Handle(GetCartQueryById request, CancellationToken cancellationToken)
        {
           
            var cartItems = await _cartRepo.GetAllAsync();

            if (cartItems == null || !cartItems.Any())
                throw new InvalidOperationException("No items found for this user.");

            var itemIds = cartItems.Select(x => x.ProductItemId);
            var productItemEntities = await _productRepo.GetAllAsync(item => itemIds.Contains(item.Id));

            var cartItemDtos = cartItems.Select(cartItem =>
            {
                var productItem = productItemEntities.Single(productItem => productItem.Id == cartItem.ProductItemId);
                return cartItem.AsDto(productItem.Name,productItem.Description,productItem.Price);
            }).ToList();

            return cartItemDtos;
        }
    }
}
