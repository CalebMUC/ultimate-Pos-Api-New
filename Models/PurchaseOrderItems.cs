using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate_POS_Api.Models
{
    public class PurchaseOrderItems
    {
        [Key]
        public Guid PurchaseOrderItemId { get; set; }

        [Required]
        public Guid PurchaseOrderId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(100)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        // 🔹 Navigation property
        public PurchaseOrders PurchaseOrder { get; set; }
        public Products Product { get; set; }

    }
}
