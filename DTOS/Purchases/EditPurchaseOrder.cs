public class EditPurchaseOrder
{
    public Guid PurchaseOrderId { get; set; }
    public string? OrderNumber { get; set; }
    public Guid SupplierId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Status { get; set; }
    public string? UpdatedBy { get; set; }
    public string? Notes { get; set; }

    public List<EditPurchaseOrderItem> Items { get; set; } = new();
}

public class EditPurchaseOrderItem
{
    public Guid? PurchaseOrderItemId { get; set; } // null = new item
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public bool IsActive { get; set; } = true;
}
