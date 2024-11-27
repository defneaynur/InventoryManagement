namespace InventoryManagement.Domain.Models.DatabaseModel.Base
{
    public class BaseModel
    {
        public DateTime? Created { get; set; }
        public string? Creator { get; set; }
        public DateTime? Changed { get; set; }
        public string? Changer { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
