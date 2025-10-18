
using System.Text.Json.Serialization;

namespace Order.Service.Features.Orders.Dtos
{
    public class OrderDetailDto
    {
        [JsonPropertyName("product_id")]
        public Guid ProductId { get; set; }

        [JsonPropertyName("product_name")]
        public string ProductName { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("quality")]
        public int Quantity { get; set; }
    }
}
