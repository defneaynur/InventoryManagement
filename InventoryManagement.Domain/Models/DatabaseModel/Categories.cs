using InventoryManagement.Domain.Models.DatabaseModel.Base;

namespace InventoryManagement.Domain.Models.DatabaseModel
{
    public class Categories : BaseModel
    {
        public long Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
