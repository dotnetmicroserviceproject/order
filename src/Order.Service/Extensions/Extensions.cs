using Order.Service.Entities;
using Order.Service.Features.CartItems.Dtos;

namespace Order.Service.Extensions
{
    public static class Extensions
    {
        public static CartDto AsDto(this CartItem item , string name, string description, decimal price)
        {
            return new CartDto
            {
                ProductId = item.ProductItemId,
                Quantity = item.Quantity,
                CreatedDate = item.CreatedDate,
                ProductName = name,
                ProductDescription = description,
                Price = price 
            };
        }
    }
}
