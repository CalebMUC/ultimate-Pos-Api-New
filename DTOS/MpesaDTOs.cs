using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Ultimate_POS_Api.DTOS
{
    public class MpesaDTOs
    {
        // No properties in this class, so nothing to change
    }

    public class MpesaPaymentRequest
    {
        public decimal Amount { get; set; } // Should map to `decimal` in PostgreSQL
        public string PhoneNumber { get; set; } // Will map to `VARCHAR` in PostgreSQL
        public string TransactionDesc { get; set; } // Corrected case to match C# convention
        public string AccountReference { get; set; } // Corrected case
    }

    public class TokenRequest
    {
        public string Token { get; set; } // Corrected the class declaration
    }

    public class ValidationRequest
    {
        public string TransactionId { get; set; } // Will map to `VARCHAR` in PostgreSQL
        public decimal Amount { get; set; } // `decimal` type for monetary values
        public string PhoneNumber { get; set; } // `VARCHAR` type in PostgreSQL
    }

    public class RegisterUrlsRequest
    {
        public string ShortCode { get; set; } // `VARCHAR`
        public string ResponseType { get; set; } = "Completed"; // Default to "Completed"
        public string ConfirmationURL { get; set; } // `VARCHAR`
        public string ValidationURL { get; set; } // `VARCHAR`
    }

    public class SimulateC2BRequest
    {
        public string ShortCode { get; set; } // `VARCHAR`
        public string CommandID { get; set; } = "CustomerPayBillOnline"; // Default fixed string
        public decimal Amount { get; set; } // `decimal` for amount
        public string Msisdn { get; set; } // Customer phone number (`VARCHAR`)
        public string BillRefNumber { get; set; } // Optional reference (`VARCHAR`)
    }

    public class MpesaCallbackData
    {
        public string ResultCode { get; set; } // `VARCHAR`
        public string ResultDesc { get; set; } // `VARCHAR`
        public string MerchantRequestID { get; set; } // `VARCHAR`
        public string CheckoutRequestID { get; set; } // `VARCHAR`
        public decimal Amount { get; set; } // `decimal`
        public string PhoneNumber { get; set; } // `VARCHAR`
        public string TransactionDate { get; set; } // `VARCHAR`, could be converted to a `DateTime` if needed
    }

    public class MpesaTransactionData
    {
        public string TransactionID { get; set; } // `VARCHAR`
        public string PaymentID { get; set; } // `VARCHAR`
        public List<MpesaPaymentRequest> MpesaRequestData { get; set; } // Collection of requests
        public List<MpesaCallbackData> MpesaCallbackData { get; set; } // Collection of callback data
    }

    public class MpesaRequestListDto
    {
        [Required]
        public IList<MpesaPaymentRequest> Mpesa { get; set; } = new List<MpesaPaymentRequest>(); // List of payment requests
    }

    public class MpesaTokenListDto
    {
        [Required]
        public IList<TokenRequest> Mpesa { get; set; } = new List<TokenRequest>(); // List of token requests
    }
}
