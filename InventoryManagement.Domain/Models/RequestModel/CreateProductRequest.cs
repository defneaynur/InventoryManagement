using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Domain.Models.RequestModel
{
    public class CreateProductRequest
    {
        public long CategoryId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public int MinQuantityValue { get; set; }
        public int MaxCapacity { get; set; }
        public DateTime? Created { get; set; }
        public string Creator { get; set; }
    }
}
