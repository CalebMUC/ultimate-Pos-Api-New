using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate_POS_Api.Models
{
   public class Accounts
   {
      [Key]
      public Guid AccountId { get; set; }
      [ForeignKey("User")]
      public Guid UserID { get; set; }
      public User User { get; set; }
      [Required]
      [Column(TypeName = "DECIMAL(18, 2)")]
      public decimal ClearBalance { get; set; }
      [Required]
      [Column(TypeName = "DECIMAL(18, 2)")]
      public decimal OpeningBalance { get; set; }


      // [Required]
      // [Column(TypeName = "DECIMAL(18, 2)")]
      // public decimal ClosingBalance { get; set; }

      [Required]
      public bool AccountStatus { get; set; }

      // [Required]
      // [Column(TypeName = "VARCHAR(255)")]
      // public string BalanceStatus { get; set; }

      [Required]
      [Column(TypeName = "VARCHAR(255)")]
      public string CreatedBy { get; set; }
      [Required]
      public DateTime CreatedOn { get; set; }
      public string UpdatedBy { get; set; }
      public DateTime? UpdatedOn { get; set; }

   }

   public class AccountTrxSettlement
   {
      [Key]
      public Guid SettlementId { get; set; }

      [Required]
      public DateTime DateTime { get; set; } // Timestamp of when the settlement occurred

      [Required]
      public Guid AccountId { get; set; }

      [Required]
      public Guid UserId { get; set; }

      [Required]
      [Column(TypeName = "decimal(18,2)")]
      public decimal SettledOpeningBalance { get; set; }

      [Required]
      [Column(TypeName = "decimal(18,2)")]
      public decimal SettledClosingBalance { get; set; }

      [Required]
      [MaxLength(50)]
      public bool SettledAccountStatus { get; set; }

      // Optional fields for debugging or analytics
      public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
   }
}