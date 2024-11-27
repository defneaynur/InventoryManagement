using InventoryManagement.Domain.Models.DatabaseModel.Base;

namespace InventoryManagement.Domain.Models.DatabaseModel
{
    public class Products : BaseModel
    {
        public long Id { get; set; }
        public long CategoryId { get; set; }
        public string? ProductName { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public int MinQuantityValue { get; set; }
        public int MaxCapacity { get; set; }

    }
}
