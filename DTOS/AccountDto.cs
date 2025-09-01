using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate_POS_Api.DTOS
{
   public class AccountDto
   {
      public Guid UserID { get; set; }

      // public Guid AccountId { get; set; }
      // public decimal ClearBalance { get; set; }
      // public decimal OpeningBalance { get; set; }
      // public decimal ClosingBalance { get; set; }
      // public bool AccountStatus { get; set; }
      // public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
      // public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
      // public required string CreatedBy { get; set; }
      // public string? UpdatedBy { get; set; }

   }

   public class UpdateAccountDTO
   {

      public Guid AccountId { get; set; }
      public decimal Amount { get; set; }

      // public decimal ClearBalance { get; set; }
      // public decimal OpeningBalance { get; set; }
      // public decimal ClosingBalance { get; set; }
   }

   public class OpenCloseAccountDto
   {
      public Guid AccountId { get; set; }
      public bool AccountStatus { get; set; }
   }

   public class AccountTrxSettlementDto
   {
      public DateTime DateTime { get; set; }
      public Guid AccountId { get; set; }
      public Guid UserId { get; set; }

      public decimal SettledOpeningBalance { get; set; }
      public decimal SettledClosingBalance { get; set; }

      public bool SettledAccountStatus { get; set; }

      public DateTime SettlementDateTime { get; set; }

      public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
   }
}