namespace Order.Service.Features.CartItems.Dtos
{
    public record CartItemDto
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
