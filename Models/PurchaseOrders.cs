using System.ComponentModel.DataAnnotations;

namespace Ultimate_POS_Api.Models
{
    public class PurchaseOrders
    {
        [Key]
        [Required]
        public Guid purchaseOrderId { get; set; }
        public string OrderNumber { get; set; }
        public Guid SupplierId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public bool isActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string Notes { get; set; }
    }
}
