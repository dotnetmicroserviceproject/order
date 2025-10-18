using System.Text.Json.Serialization;

namespace Order.Service.Features.CartItems.Dtos
{
    public class CartResponseDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } 

        [JsonPropertyName("product_id")]
        public Guid ProductItemId { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

    }
}
