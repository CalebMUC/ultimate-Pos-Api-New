using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate_POS_Api.Models
{
   public class Logs
   {
      [Key]
      public Guid Id { get; set; }
      public string Module { get; set; }
      public string UserID { get; set; }
      public DateTime Time { get; set; }
      public string Request { get; set; }
      public string Status { get; set; }
      public string Response { get; set; }
   }

}