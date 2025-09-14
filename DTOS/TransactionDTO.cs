using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Ultimate_POS_Api.DTOS
{
    public class TransactionDto
    {
        // public Guid TransactionID { get; set; } 
        //public Guid UserID { get; set; }
        public decimal TotalValueAddedTax { get; set; }
        public decimal TotalCost { get; set; } = 0;
        public decimal TotalDiscount { get; set; } = 0;
        public decimal AmountRecieved { get; set; } = 0;
        public decimal CashChange { get; set; } = 0;
        // Relationship with transaction products and payment details
        public List<TransactionProductDto> transactionproducts { get; set; } = new List<TransactionProductDto>();
        public List<PaymentDetailsDto> PaymentDetails { get; set; } = new List<PaymentDetailsDto>();
    }

    public class PaymentDetailsDto
    {
        // public string PaymentID { get; set; } = string.Empty; 
        public int PaymentMethodId { get; set; } 
        public int PaymentStatus { get; set; }
        public string PaymentReference { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string TransactionID { get; set; } = string.Empty;
        // public DateTime PaymentDate { get; set; } = DateTime.UtcNow; /
    }

    public class TransactionProductDto
    {
        public Guid ProductID { get; set; }
        public string SKU { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal SubTotal { get; set; }
    }

    public class TransactionListDto
    {
        [Required]
        public IList<TransactionDto> transaction { get; set; } = new List<TransactionDto>();
    }
}
