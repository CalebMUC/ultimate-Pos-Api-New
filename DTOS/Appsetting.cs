using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate_POS_Api.DTOS
{
   public class Appsetting
   {


   }
   public class FluxApiSettings
   {
      public string ApiUrl { get; set; } = string.Empty;
      public string Username { get; set; } = string.Empty;
      public string SenderId { get; set; } = string.Empty;
      public string ConsumerKey { get; set; } = string.Empty;
      public string ConsumerSecret { get; set; } = string.Empty;
   }

   public class EmailSettings
   {
      public string SmtpServer { get; set; } = string.Empty;
      public int Port { get; set; }
      public string SenderEmail { get; set; } = string.Empty;
      public string SenderName { get; set; } = string.Empty;
   }

   public class MpesaSettings
   {
      public string ConsumerKey { get; set; } = string.Empty;
      public string ConsumerSecret { get; set; } = string.Empty;
      public string ShortCode { get; set; } = string.Empty;
      public string ValidationUrl { get; set; } = string.Empty;
      public string ConfirmationUrl { get; set; } = string.Empty;
   }

}

