using InventoryManagement.Domain.Models.DatabaseModel;

namespace InventoryManagement.Domain.Models.ResponseModel
{
    public class ProductResponse: Products
    {
        public string Category { get; set; }
    }
}
