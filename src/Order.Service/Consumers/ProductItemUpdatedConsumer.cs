using common.MongoDB.Interface;
using MassTransit;
using Order.Service.Entities;
using Product.Contracts;


namespace Order.Service.Consumers
{
    public class ProductItemUpdatedConsumer : IConsumer<ProductItemUpdated>
    {
        private readonly IMongoRepository<ProductItem> _repository;
        public ProductItemUpdatedConsumer(IMongoRepository<ProductItem> repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<ProductItemUpdated> context)
        {
            var message = context.Message;

            var item = await _repository.GetAsync(message.ItemId);

            if (item == null)
            {
                item = new ProductItem
                {
                    Id = message.ItemId,
                    Name = message.Name,
                    Description = message.Description,
                    Price = message.Price,
                    StockQuantity = message.StockQuantity,
                };
                await _repository.CreateAsync(item);
            }
            else
            {
                item.Name = message.Name;
                item.Description = message.Description;

                await _repository.UpdateAsync(item);
            }
           
        }
    }
}
