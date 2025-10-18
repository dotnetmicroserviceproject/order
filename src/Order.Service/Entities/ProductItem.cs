using common.Entities;

namespace Order.Service.Entities
{
    public class ProductItem: EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
