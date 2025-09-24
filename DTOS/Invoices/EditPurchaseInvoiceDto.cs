namespace Ultimate_POS_Api.DTOS.Invoices
{
    public class EditPurchaseInvoiceDto
    {
        public Guid PurchaseInvoiceId { get; set; }
        public string? InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TaxAmount { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
