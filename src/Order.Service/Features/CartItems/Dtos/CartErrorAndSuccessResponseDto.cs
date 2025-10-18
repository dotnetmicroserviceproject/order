using System.Text.Json.Serialization;

namespace Order.Service.Features.CartItems.Dtos
{
    public class CartErrorAndSuccessResponseDto<T>
    {
        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("data")]
        public T Data { get; set; }
    }
}
