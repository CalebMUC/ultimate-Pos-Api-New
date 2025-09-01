using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ultimate_POS_Api.Models
{
   public class BusinessDetail
   {
      [Key]
      public Guid BusinessId { get; set; } // Primary Key
      public string BusinessName { get; set; }
      public string Industry { get; set; } // Or NatureOfBusiness
      public string PhysicalAddress { get; set; }
      public string PhoneNumber { get; set; }
      public string KRAPIN { get; set; }
      public int NumberOfEmployees { get; set; }
      public string MpesaTill { get; set; }
      public string BankAccountNo { get; set; }
      public string BusinessEmail { get; set; }

      // Audit Fields
      public string CreatedBy { get; set; }
      public DateTime CreatedOn { get; set; }
      public string UpdatedBy { get; set; }
      public DateTime? UpdatedOn { get; set; }

   }
}