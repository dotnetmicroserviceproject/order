using common.MongoDB.Interface;
using MassTransit;
using Order.Contracts;
using Order.Service.Entities;
using Order.Service.Exceptions;



namespace Order.Service.Consumers
{
    public class GrantItemConsumer : IConsumer<GrantItem>
    {
        private readonly IMongoRepository<CartItem> _cartRepo;
        private readonly IMongoRepository<ProductItem> _productRepo;
        public GrantItemConsumer(IMongoRepository<CartItem> cartRepo, IMongoRepository<ProductItem> productRepo)
        {
            _cartRepo = cartRepo;
            _productRepo = productRepo;
        }
        public async Task Consume(ConsumeContext<GrantItem> context)
        {
            var message = context.Message;
            var product = await _productRepo.GetAsync(message.ProductId);
            if(product == null)
            {
                throw new UnknownItemException(message.ProductId); 
            }

            // 3️⃣ Check if cart item already exists
            var existingItem = await _cartRepo
                .GetAsync(x => x.UserId == message.UserId && x.ProductItemId ==message.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += message.Quantity;
                await _cartRepo.UpdateAsync(existingItem);
            }
            else
            {
                var newItem = new CartItem
                {
                    UserId = message.UserId,
                    ProductItemId = message.ProductId,
                    Quantity = message.Quantity,
                    
                };
                await _cartRepo.CreateAsync(newItem);
            }

            await context.Publish(new CartItemGranted(message.CorrelationId));
        }
    }
}
