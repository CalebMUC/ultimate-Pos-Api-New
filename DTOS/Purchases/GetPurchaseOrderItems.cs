namespace Ultimate_POS_Api.DTOS.Purchases
{
    public class GetPurchaseOrderItems
    {
        public Guid purchaseOrderItemId { get; set; }
        public Guid purchaseOrderId { get; set; }
        public Guid productId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public bool isActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
