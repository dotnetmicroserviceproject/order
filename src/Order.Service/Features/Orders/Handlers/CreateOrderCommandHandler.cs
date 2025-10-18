using AutoMapper;
using common.MongoDB.Interface;
using MassTransit.Initializers;
using MediatR;
using Order.Service.Entities;
using Order.Service.Features.Orders.Commands;
using Order.Service.Features.Orders.Dtos;


namespace Order.Service.Features.Orders.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderSuccessAndErrorResponseDto<OrderHeaderResponseDto>>
    {
        private readonly IMongoRepository<OrderItem> _orderHeaderRepo;
        private readonly IMongoRepository<OrderDetails> _orderDetailsRepo;
        private readonly IMongoRepository<CartItem> _CartItemRepo;
        private readonly IMongoRepository<UserItem> _userRepo;
        private readonly IMongoRepository<ProductItem> _productRepo;
        private readonly IMapper _mapper;
        public CreateOrderCommandHandler(
               IMongoRepository<OrderItem> orderHeaderRepo,
               IMongoRepository<OrderDetails> orderDetailsRepo,
               IMapper mapper,
               IMongoRepository<CartItem> cartItemRepo,
               IMongoRepository<UserItem> userRepo,
               IMongoRepository<ProductItem> productRepo
             )
        {
            _orderHeaderRepo = orderHeaderRepo;
            _orderDetailsRepo = orderDetailsRepo;
            _mapper = mapper;
            _CartItemRepo = cartItemRepo;
            _userRepo = userRepo;
            _productRepo = productRepo;
        }
        public async Task<OrderSuccessAndErrorResponseDto<OrderHeaderResponseDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var userId = await _userRepo.GetAsync(x=> x.Id == request.OrderHeaderCreateDto.UserId);
            if (userId == null)
                return new OrderSuccessAndErrorResponseDto<OrderHeaderResponseDto>
                {
                    StatusCode = 400,
                    Message = "User not found",

                };

            //  Get cart items
            var cartItems = await _CartItemRepo.GetAllAsync(x => x.UserId == request.OrderHeaderCreateDto.UserId);
            if (cartItems == null || !cartItems.Any())
                return new OrderSuccessAndErrorResponseDto<OrderHeaderResponseDto>
                {
                    StatusCode = 404,
                    Message = "Cart is empty. Cannot create an order.",
                    Data = null

                };

            // Get all product
            var productIds = cartItems.Select(c => c.ProductItemId).ToList();
            var products = await _productRepo.GetAllAsync(p => productIds.Contains(p.Id));

            if (products == null || !products.Any())
                return new OrderSuccessAndErrorResponseDto<OrderHeaderResponseDto>
                {
                    StatusCode = 404,
                    Message = "Some products in the cart no longer exist",
                    Data = null

                };

            // Validate stock availability
            foreach (var cartItem in cartItems)
            {
                var product = products.FirstOrDefault(p => p.Id == cartItem.ProductItemId);

                if (product == null)
                {
                    return new OrderSuccessAndErrorResponseDto<OrderHeaderResponseDto>
                    {
                        StatusCode = 404,
                        Message = $"Product with ID {cartItem.ProductItemId} not found."
                    };
                }

                if (product.StockQuantity < cartItem.Quantity)
                {
                    return new OrderSuccessAndErrorResponseDto<OrderHeaderResponseDto>
                    {
                        StatusCode = 409,
                        Message = $"Insufficient stock for product '{product.Name}'. Requested: {cartItem.Quantity}, Available: {product.StockQuantity}."
                    };
                }
            }

            // Deduct stock
            foreach (var cartItem in cartItems)
            {
                var product = products.First(p => p.Id == cartItem.ProductItemId);
                product.StockQuantity -= cartItem.Quantity;
                await _productRepo.UpdateAsync(product);
            }

            // Calculate totals
            var totalItems = cartItems.Sum(c => c.Quantity);

            var totalPrice = cartItems.Sum(c =>
            {
                var product = products.FirstOrDefault(p => p.Id == c.ProductItemId);
                return product != null ? c.Quantity * product.Price : 0;
            });

            var orderHeader = _mapper.Map<OrderItem>(request.OrderHeaderCreateDto);
            orderHeader.UserId = request.OrderHeaderCreateDto.UserId;
            orderHeader.TotalItems = totalItems;
            orderHeader.OrderTotal = totalPrice;
            orderHeader.OrderDate = DateTime.UtcNow;
            orderHeader.Status = OrderStatus.CONFIRMED;

             await _orderHeaderRepo.CreateAsync(orderHeader);

            // Map cart items to order details
            var orderDetails = cartItems.Select(item =>
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductItemId);
                return new OrderDetails
                {
                    ProductId = item.ProductItemId,
                    Quantity = item.Quantity,
                    Price = product?.Price ?? 0,
                    OrderItemId = orderHeader.Id
                };
            }).ToList();

            foreach (var detail in orderDetails)
            {
                await _orderDetailsRepo.CreateAsync(detail);
            }

            //await _orderDetailsRepo.AddRangeAsync(orderDetails);
            //await _orderDetailsRepo.SaveChanges();

            orderHeader.OrderDetails = orderDetails;

            // ✅ Clear cart
            foreach (var item in cartItems)
            {
                await _CartItemRepo.DeleteAsync(item);
            }

            //await _CartItemRepo.DeleteRangeAsync(cartItems);
            //await _CartItemRepo.SaveChanges();
            var response = _mapper.Map<OrderHeaderResponseDto>(orderHeader);

            return new OrderSuccessAndErrorResponseDto<OrderHeaderResponseDto>
            {
                StatusCode = 200,
                Message = "order added successfully!!!",
                Data = response
            };
        }
    }
}
