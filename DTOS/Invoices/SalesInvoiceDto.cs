namespace Ultimate_POS_Api.DTOS.Invoices
{
    public class SalesInvoiceDto
    {
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public string? Notes { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public ICollection<SalesInvoiceItemDto> Items { get; set; } = new List<SalesInvoiceItemDto>();
    }

    public class SalesInvoiceItemDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
