using System.Collections.Generic; // Import for IList
using System.ComponentModel.DataAnnotations;

namespace Ultimate_POS_Api.DTOS
{
    public class ProductDTOs
    {
        // public Guid ProductID { get; set; } 
        [Required]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        public string ProductDescription { get; set; } = string.Empty;

        public string? unit { get; set; }

        public int? Weight_Volume { get; set; }

        [Required]
        public Guid CategoryID { get; set; }

        public decimal BuyingPrice { get; set; }

        public decimal SellingPrice { get; set; }

        public int Quantity { get; set; }

        public Guid Supplier { get; set; }

    }

    // public class ProductListDto
    // {
    //     [Required]
    //     public IList<ProductDTOs> Products { get; set; } = new List<ProductDTOs>();
    // } 
    public class ProductCatalogueDTO
    {
        public Guid ProductId { get; set; }
        public Guid SKU { get; set; }
        public string ProductName { get; set; }
        public string ProductCost { get; set; }
        public Guid CategoryID { get; set; }
        public int? Weight_Volume { get; set; }
        public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; } 

        public bool Availability { get; set; }
    }

}
