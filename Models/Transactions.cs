using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate_POS_Api.Models
{
    public class Transactions
    {
        [Key]
        public Guid TransactionId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public string InvoiceNumber { get; set; } // e.g. POS receipt number

        [Required]
        [Column(TypeName = "DECIMAL(18,2)")]
        public decimal TotalAmount { get; set; } // Total sale amount

        [Column(TypeName = "DECIMAL(18,2)")]
        public decimal Discount { get; set; } = 0;

        [Column(TypeName = "DECIMAL(18,2)")]
        public decimal Tax { get; set; } = 0;

        [Column(TypeName = "DECIMAL(18,2)")]
        public decimal NetAmount { get; set; } // (Total - Discount + Tax)

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        //[MaxLength(100)]
        //public string CustomerName { get; set; } // Optional (walk-in can be null)
        [ForeignKey("User")]
        public Guid UserID { get; set; }

        [MaxLength(50)]
        public string Cashier { get; set; } // User handling the transaction

        public int TillId { get; set; }
        public bool IsCancelled { get; set; } = false;

        // 🔗 Relationships
        public ICollection<TransactionProducts> TransactionProducts { get; set; } // Items sold
        public ICollection<PaymentDetails> Payments { get; set; } // Payments made
        public User User { get; set; }
    }
}

