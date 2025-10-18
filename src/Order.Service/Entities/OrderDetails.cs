using common.Entities;


namespace Order.Service.Entities
{
    public class OrderDetails : EntityBase
    {
        public Guid OrderItemId { get; set; }

        public OrderItem OrderItem { get; set; }

        public Guid ProductId { get; set; }


        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
