using common.MongoDB.Interface;
using MassTransit;
using Order.Service.Entities;
using User.Contracts;

namespace Order.Service.Consumers
{
    public class UserCreatedConsumer : IConsumer<ApplicationUserCreated>
    {
        private readonly IMongoRepository<UserItem> _repository;
        public UserCreatedConsumer(IMongoRepository<UserItem> repository)
        {
            _repository = repository;
        }
        public async Task Consume(ConsumeContext<ApplicationUserCreated> context)
        {
            var message = context.Message;

            var user = await _repository.GetAsync(message.UserId);

            if (user != null)
            {
                return;
            }
            user = new UserItem
            {
                Id = message.UserId,
                Email = message.Email,
            };
            await _repository.CreateAsync(user);
        }
    }
}
