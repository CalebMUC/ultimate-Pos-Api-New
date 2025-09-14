using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Ultimate_POS_Api.Models;

public class Products
{
    [Key]
    public Guid ProductID { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR(255)")]
    public string ProductName { get; set; } = string.Empty;

    [Column(TypeName = "TEXT")]
    public string ProductDescription { get; set; } = string.Empty;

    [Column(TypeName = "VARCHAR(100)")]
    public string? SKU { get; set; }

    [Column(TypeName = "VARCHAR(100)")]
    public string? Barcode { get; set; }

    [Column(TypeName = "VARCHAR(10)")]
    public string Unit { get; set; }

    [Column(TypeName = "VARCHAR(255)")]
    public int? Weight_Volume { get; set; }

    [Required]
    public Guid CategoryID { get; set; }
    public Categories Categories { get; set; }

    [Required]
    public Guid SupplierId { get; set; }
    public Supplier Supplier { get; set; }

    [Column(TypeName = "DECIMAL(10, 2)")]
    public decimal BuyingPrice { get; set; }

    [Column(TypeName = "DECIMAL(10, 2)")]
    public decimal SellingPrice { get; set; }

    [Column(TypeName = "DECIMAL(10, 2)")]
    public decimal? DiscountPrice { get; set; }

    public int Quantity { get; set; }
    public int ReorderLevel { get; set; }

    public bool Status { get; set; }
    public bool IsDeleted { get; set; } = false;

    public string? brand { get; set; }


    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "VARCHAR(255)")]
    public required string CreatedBy { get; set; }

    [Column(TypeName = "VARCHAR(255)")]
    public string? UpdatedBy { get; set; }

    public ICollection<Catalogue> Catalogues { get; set; }
}
