using System.ComponentModel.DataAnnotations;

namespace Ultimate_POS_Api.Models
{
    public class PurchaseInvoices
    {
        [Key]
        public Guid purchaseInvoiceId { get; set; }
        public Guid purchaseOrderId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public bool isActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
