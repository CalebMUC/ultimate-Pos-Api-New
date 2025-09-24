using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate_POS_Api.Models
{
    public class PurchaseOrders
    {
        [Key]
        public Guid PurchaseOrderId { get; set; }

        [MaxLength(50)]
        public string OrderNumber { get; set; }

        [Required]
        public Guid SupplierId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [MaxLength(30)]
        public string Status { get; set; } = "Pending";

        public bool IsActive { get; set; } = true;

        [MaxLength(100)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        // 🔹 Navigation property
        // 🔹 Navigation property
        public Supplier Supplier { get; set; }
        public ICollection<PurchaseOrderItems> Items { get; set; } = new List<PurchaseOrderItems>();
        public ICollection<PurchaseInvoices> Invoices { get; set; } = new List<PurchaseInvoices>();
    }
}
