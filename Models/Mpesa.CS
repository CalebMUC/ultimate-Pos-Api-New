using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate_POS_Api.Models
{
    public class Mpesa
    {
        public string token { get; set; }

    }

    //   public class MpesaPayment
    //         {
    //             public decimal Amount { get; set; }
    //             public string PhoneNumber { get; set; }
    //             public string transactionDesc { get; set; }
    //             public string accountReference { get; set; }
    //         }

    public class Tokenmpesa()
    {
        public string token { get; set; }
    }




    // public class MpesaCallback
    // {
    //     public string ResultCode { get; set; }
    //     public string ResultDesc { get; set; }
    //     public string MerchantRequestID { get; set; }
    //     public string CheckoutRequestID { get; set; }
    //     public decimal Amount { get; set; }
    //     public string PhoneNumber { get; set; }
    //     public string TransactionDate { get; set; }
    // } 

    //     public class MpesaTransactionData()
    //     {
    //     //public List<TransactionDto> transactiondetails { get; set; }
    //     public String TransactionID { get; set; }

    //     //public List<PaymentDetailsDto> PaymentDetails { get; set; }
    //     public string PaymentID { get; set; }

    //     public List<MpesaPaymentRequest> mpesarequestdata { get; set; }

    //     public List<MpesaCallbackData> mpesacallbackdata { get; set; }

    // }


    // public class MpesaRequestListDto
    // {
    //     [Required]
    //     public IList<MpesaPaymentRequest> mpesa { get; set; } = new List<MpesaPaymentRequest>();
    // }

    //  public class MpesaTokenListDto
    // {
    //     [Required]
    //     public IList<TokenRequest> mpesa { get; set; } = new List<TokenRequest>();
    // }

}

