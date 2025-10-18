using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Order.Service.Features.Orders.Dtos
{
    public class OrderHeaderCreateDto
    {
        [Required]
        [JsonPropertyName("user_id")]
        public Guid UserId { get; set; }

        [Required]
        [JsonPropertyName("pickup_name")]
        public string PickupName { get; set; }

        [Required]
        [JsonPropertyName("pickup_phonenumber")]
        public string PickupPhoneNumber { get; set; }

        [JsonPropertyName("pickup_email")]
        public string PickupEmail { get; set; }

    }

}
