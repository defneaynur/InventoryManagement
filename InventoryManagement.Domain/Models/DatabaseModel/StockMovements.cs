using InventoryManagement.Domain.Models.Enums;

namespace InventoryManagement.Domain.Models.DatabaseModel
{
    public class StockMovements
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public int OldQuantity { get; set; }
        public int CurrentQuantity { get; set; }
        public string Note { get; set; }
        public MovementType MovementType { get; set; }
        public DateTime? Created { get; set; }
        public string Creator { get; set; }
    }

}
