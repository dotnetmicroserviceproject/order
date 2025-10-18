using common.Entities;
using MongoDB.Bson.Serialization.Attributes;
using Order.Service.Features.Orders.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.Service.Entities
{
    public class OrderItem : EntityBase
    {
        public string PickupName { get; set; }

        [Required]
        public string PickupPhoneNumber { get; set; }

        public string PickupEmail { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        [BsonDefaultValue(OrderStatus.PENDING)]
        public OrderStatus Status { get; set; } = OrderStatus.PENDING;

        public Guid UserId { get; set; }
        public decimal OrderTotal { get; set; }
        public int TotalItems { get; set; }
        public DateTime OrderDate { get; set; }

        public ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
    }

}
