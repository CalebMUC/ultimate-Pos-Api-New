namespace Ultimate_POS_Api.DTOS.Invoices
{
    public class PurchasesInvoiceDto
    {
        public Guid PurchaseOrderId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public string? Notes { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}
