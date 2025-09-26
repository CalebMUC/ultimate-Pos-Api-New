namespace Ultimate_POS_Api.DTOS.Invoices
{
    public class EditSalesInvoiceDto
    {
        public Guid SalesInvoiceId { get; set; }   // Required to find invoice

        public DateTime? InvoiceDate { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TaxAmount { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }

        public string UpdatedBy { get; set; } = string.Empty;

        // Optional — if items need editing
        public ICollection<EditSalesInvoiceItemDto>? Items { get; set; }
    }

    public class EditSalesInvoiceItemDto
    {
        public Guid? SalesInvoiceItemId { get; set; } // null means new item
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
