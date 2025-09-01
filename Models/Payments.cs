using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate_POS_Api.Models
{
    // Payment Method (Cash, Mpesa, Card, etc.)
    public class Payments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentMethodId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<PaymentDetails> PaymentDetails { get; set; }
    }

    // Payment instance for a specific transaction
    public class PaymentDetails
    {
        [Key]
        public Guid PaymentDetailId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid TransactionId { get; set; } // FK → Transactions

        [Required]
        [ForeignKey("Payments")]
        public int PaymentMethodId { get; set; } // FK → Payments

        public string Name { get; set; }

        [Required]
        [Column(TypeName = "DECIMAL(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentReference { get; set; } // e.g. Mpesa code, receipt number

        [Required]
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        // Navigation
        public Payments PaymentMethod { get; set; }
        public Transactions Transaction { get; set; }
    }

    public enum PaymentStatus
    {
        Pending = 0,
        Completed = 1,
        Failed = 2,
        Refunded = 3
    }
}
