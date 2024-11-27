using InventoryManagement.Domain.Models.DatabaseModel;

namespace InventoryManagement.Domain.Models.ResponseModel
{
    public class StockMovementsResponse : StockMovements
    {
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public long CategoryId { get; set; }
    }
}
