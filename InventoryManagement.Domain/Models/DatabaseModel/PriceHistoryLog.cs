namespace InventoryManagement.Domain.Models.DatabaseModel
{
    public class PriceHistoryLog
    {
        public long Id { get; set; } 
        public long ProductId { get; set; } 
        public decimal CostPrice { get; set; } 
        public decimal OldSalePrice { get; set; } 
        public decimal? CurrentSalePrice { get; set; }
        public string Note { get; set; }
        public DateTime? Created { get; set; } 
        public string Creator { get; set; } 
    }
}
