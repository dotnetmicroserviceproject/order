using System.Text.Json.Serialization;


namespace Order.Service.Features.Orders.Dtos
{
    public class OrderHeaderResponseDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("pickup_name")]
        public string PickupName { get; set; }

        [JsonPropertyName("pickup_phonenumber")]
        public string PickupPhoneNumber { get; set; }

        [JsonPropertyName("pickup_email")]
        public string PickupEmail { get; set; }

        [JsonPropertyName("order_total")]
        public decimal OrderTotal { get; set; }

        [JsonPropertyName("status")]
        public OrderStatus Status { get; set; }

        [JsonPropertyName("order_date")]
        public DateTime OrderDate { get; set; }

        [JsonPropertyName("order_details")]
        public List<OrderDetailDto> OrderDetails { get; set; }
    }

}
