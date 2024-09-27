using InventoryManagement.Domain.Models.DatabaseModel;

namespace InventoryManagement.Domain.Models.ResponseModel
{
    public class PriceHistoryResponse : PriceHistoryLog
    {
        public string ProductName { get; set; }
    }
}
