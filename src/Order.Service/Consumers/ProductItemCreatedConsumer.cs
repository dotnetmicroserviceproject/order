using common.MongoDB.Interface;
using MassTransit;
using Order.Service.Entities;
using Product.Contracts;


namespace Order.Service.Consumers
{
    //this consumer is from my productservice
    public class ProductItemCreatedConsumer : IConsumer<ProductItemCreated>
    {
        private readonly IMongoRepository<ProductItem> _repository;
        public ProductItemCreatedConsumer(IMongoRepository<ProductItem> repository)
        {
            _repository = repository;
        }
        public async Task Consume(ConsumeContext<ProductItemCreated> context)
        {
            var message = context.Message;

            var item = await _repository.GetAsync(message.ItemId);

            if (item != null)
            {
                return;
            }

            item = new ProductItem
            {
                Id = message.ItemId ,
                Name = message.Name ,
                Description = message.Description ,
                Price = message.Price ,
                StockQuantity = message.StockQuantity ,

            };
            await _repository.CreateAsync(item);
        }
    }
}
