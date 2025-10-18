namespace Order.Service.Exceptions
{
    [Serializable]
    internal class UnknownItemException : Exception
    {
        public Guid ProductId { get; }

        public UnknownItemException(Guid productId) :
            base($"Unkonwn item `{productId}`")
        {
            this.ProductId = productId;
        }
    }
}