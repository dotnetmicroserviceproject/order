using common.Entities;

namespace Order.Service.Entities
{
    public class CartItem : EntityBase
    {
        public Guid UserId { get; set; }
        public Guid ProductItemId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
