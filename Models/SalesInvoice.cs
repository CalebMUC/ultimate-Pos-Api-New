using System.ComponentModel.DataAnnotations;

namespace Ultimate_POS_Api.Models
{
    public class SalesInvoice
    {
        [Key]
        public Guid SalesInvoiceId { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public string Status { get; set; } = "Pending"; // Default
        public string? Notes { get; set; }
        public bool IsActive { get; set; } = true;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        // Navigation
        public ICollection<SalesInvoiceItem> SalesInvoiceItems { get; set; } = new List<SalesInvoiceItem>();
    }

    public class SalesInvoiceItem
    {
        [Key]
        public Guid SalesInvoiceItemId { get; set; }

        public Guid SalesInvoiceId { get; set; }

        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }

        // Navigation
        public SalesInvoice SalesInvoice { get; set; }
    }
}
