using System.Collections.Generic; // Import for IList
using System.ComponentModel.DataAnnotations;

namespace Ultimate_POS_Api.DTOS
{
    public class ProductDTOs
    {
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public string? SKU { get; set; }
        public string? Barcode { get; set; }
        public string Unit { get; set; } = string.Empty;
        public int? Weight_Volume { get; set; }

        public Guid CategoryID { get; set; }
        public Guid SupplierId { get; set; }

        public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal? DiscountPrice { get; set; }

        public int Quantity { get; set; }
        public int ReorderLevel { get; set; }

        public bool Status { get; set; }

        public string? Brand { get; set; }


        public string CreatedBy { get; set; } = string.Empty;

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
