using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Ultimate_POS_Api.Models;

namespace Ultimate_POS_Api.DTOS
{
    public class GetProductsDto
    {

        public Guid ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public int? Weight_Volume { get; set; }
        public Guid CategoryID { get; set; }

        public decimal BuyingPrice { get; set; }

        public decimal SellingPrice { get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;

        public required string CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public Guid SupplierId { get; set; }  // This is the foreign key
        public bool Status { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
    }
}
