using common.MongoDB.Interface;
using MassTransit;
using Order.Service.Entities;
using Product.Contracts;

namespace Order.Service.Consumers
{
    public class ProductItemDeletedConsumer : IConsumer<ProductItemDeleted>
    {
        private readonly IMongoRepository<ProductItem> _repository;
        public ProductItemDeletedConsumer(IMongoRepository<ProductItem> repository)
        {
            _repository = repository;
        }
        public async Task Consume(ConsumeContext<ProductItemDeleted> context)
        {
            var message = context.Message;

            var item = await _repository.GetAsync(message.ItemId);

            if (item == null)
            {
               return;
            }
       
                await _repository.DeleteAsync(item);
           
        }
    }
}
