using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Ultimate_POS_Api.Data;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;
using Ultimate_POS_Api.Repository;

namespace Ultimate_POS_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MpesaController : ControllerBase
    {
        private readonly IMpesaRepository _mpesaRepository;
        private readonly UltimateDBContext _dbContext;

        public MpesaController(IMpesaRepository mpesaRepository)
        {
            _mpesaRepository = mpesaRepository;
        }


        // 1. Get Access Token  
        [HttpGet("access-token")]
        [Authorize]
        public async Task<IActionResult> GetAccessToken()
        {
            try
            {
                var token = await _mpesaRepository.GetAccessToken();
                return Ok(new { access_token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // 2. Initiate Payment Request (Lipa na M-Pesa Online express endpoint)
        [HttpPost("Mpesa_Express")]
        [Authorize]
        public async Task<IActionResult> PayWithMpesaExpress(MpesaRequestListDto mpesapayload)
        {
            try
            {
                var paymentResponse = await _mpesaRepository.MpesaExpress_Transaction(mpesapayload);
                return Ok(new { response = paymentResponse });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPost("Mpesa_C2B")]
        [Authorize]
        public async Task<IActionResult> PayWithMpesaC2B(SimulateC2BRequest mpesapayload)
        {
            try
            {
                var paymentResponse = await _mpesaRepository.MpesaC2B_Transaction(mpesapayload);
                return Ok(new { response = paymentResponse });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPost("RegisterUrls")]
        [Authorize]
        public async Task<IActionResult> RegisterUrls([FromBody] RegisterUrlsRequest request)
        {
            try
            {
                var RegisterUrlResponse = await _mpesaRepository.RegisterUrls(request);
                return Ok(new { response = RegisterUrlResponse });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }



        [HttpPost("Mpesa_Validation")]
        [Authorize]
        public async Task<IActionResult> MpesaValidaion(ValidationRequest validationrequest)
        {

            try
            {
                // Validate the incoming request
                if (validationrequest.Amount <= 0 || string.IsNullOrEmpty(validationrequest.TransactionId) || string.IsNullOrEmpty(validationrequest.PhoneNumber))
                {
                    return BadRequest(new { error = "Invalid request data" });
                }

                var paymentResponse = await _mpesaRepository.MpesaValidaion(validationrequest);
                return Ok(new { response = paymentResponse });

            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });

            }
        }




        // // 3. Handle Callback from M-Pesa
        // [HttpPost("callback")]
        // public IActionResult HandleMpesaCallback([FromBody] MpesaCallbackData callbackData)
        // {
        //     if (callbackData.ResultCode == "0") // Success
        //     {
        //         var callback = new
        //         {
        //              callbackData.MerchantRequestID,
        //              callbackData.CheckoutRequestID,
        //              V = Convert.ToDecimal(callbackData.Amount), // Convert string to decimal
        //              callbackData.PhoneNumber,
        //              DateTime = DateTime.Parse(callbackData.TransactionDate),
        //              V1 = "Paid"
        //         };

        //         // var mpesatransaction = new MpesaTransactionData
        //         // {
        //             // TransactionID =
        //             // PaymentID =
        //             // mpesarequestdata = callback,
        //             // mpesacallbackdata = callback

        //             //MerchantRequestID = callbackData.MerchantRequestID,
        //             //CheckoutRequestID = callbackData.CheckoutRequestID,
        //             //Amount = Convert.ToDecimal(callbackData.Amount), // Convert string to decimal
        //             //PhoneNumber = callbackData.PhoneNumber,
        //             //TransactionDate = DateTime.Parse(callbackData.TransactionDate),
        //             ////TransactionReceipt = callbackData.TransactionReceipt,
        //             //Status = "Paid"
        //         // }



        //         // Update your order or payment status in your database
        //         //Console.WriteLine($"Payment successful: {callbackData.TransactionDate}, Amount: {callbackData.Amount}");
        //         }
        //         else
        //         {
        //             // Handle failure
        //             //Console.WriteLine($"Payment failed: {callbackData.ResultDesc}");
        //         }

        //     // Respond with OK status to Safaricom
        //     return Ok(new { message = "Callback received successfully" });
        // }





    }
}


