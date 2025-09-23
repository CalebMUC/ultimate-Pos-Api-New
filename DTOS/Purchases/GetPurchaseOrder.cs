public class GetPurchaseOrder
{
    public Guid PurchaseOrderId { get; set; }
    public string OrderNumber { get; set; }
    public Guid SupplierId { get; set; }
    public string SupplierName { get; set; }      // 🔹 New
    public string SupplierContact { get; set; }   // 🔹 New (optional)
    public string SupplierEmail { get; set; }     // 🔹 New (optional)

    public DateTime OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public string Notes { get; set; }

    // 🔹 Nested items
    public List<GetPurchaseOrderItem> Items { get; set; } = new();
}

public class GetPurchaseOrderItem
{
    public Guid PurchaseOrderItemId { get; set; }
    public Guid PurchaseOrderId { get; set; }
    public Guid ProductId { get; set; }

    public string ProductName { get; set; }   // 🔹 New (optional)
    public string ProductCode { get; set; }   // 🔹 New (optional)

    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
