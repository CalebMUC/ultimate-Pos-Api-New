using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ultimate_POS_Api.DTOS
{
   public class BusinessDetailDto
   {

      public string BusinessName { get; set; }
      public string Industry { get; set; }
      public string PhysicalAddress { get; set; }
      public string PhoneNumber { get; set; }
      public string KRAPIN { get; set; }
      public int NumberOfEmployees { get; set; }
      public string MpesaTill { get; set; }
      public string BankAccountNo { get; set; }
      public string BusinessEmail { get; set; }

   }
}