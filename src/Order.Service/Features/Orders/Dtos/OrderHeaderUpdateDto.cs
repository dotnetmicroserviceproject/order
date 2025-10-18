using System.Text.Json.Serialization;

namespace Order.Service.Features.Orders.Dtos
{
    public class OrderHeaderUpdateDto
    {

        [JsonPropertyName("pickup_name")]
        public string PickupName { get; set; }

        [JsonPropertyName("pickup_phonenumber")]
        public string PickupPhoneNumber { get; set; }

        [JsonPropertyName("pickup_email")]
        public string PickupEmail { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
