namespace Order.Contracts
{
    
        public record GrantItem(Guid UserId, Guid ProductId, int Quantity ,Guid CorrelationId);

        public record CartItemGranted( Guid CorrelationId);
        public record SubtractItems(Guid UserId, Guid ProductId, int Quantity, Guid CorrelationId);

        public record CartItemSubtracted( Guid CorrelationId);

        public record CartItemUpdated(
           Guid UserId,
           Guid ProductItemId,
           int NewTotalQuantity
        );

    public record CartItemDeleted(Guid CartItemId);
    
}
