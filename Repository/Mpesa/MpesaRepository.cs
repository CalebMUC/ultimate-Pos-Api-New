using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;



namespace Ultimate_POS_Api.Repository
{
    public class MpesaRepository : IMpesaRepository
    {
        private const string MpesaApiUrl = "https://sandbox.safaricom.co.ke";
        private const string MpesaConsumerKey = "Rekz3F41KftZTvg2hIAHd1zHGvLReV0IMzyGVe5DfAB0Abxa";
        private const string MpesaConsumerSecret = "FJ2Rc0yHSNW2X72Gg01Vc2M3VfuNRW2l9ObEtxeBbFVlE8lO7r52ZjOxMrQofhoi";
        private const string LipaNaMpesaShortcode = "174379";  //sandbox



        public async Task<ResponseStatus> MpesaExpress_Transaction(MpesaRequestListDto mpesapayload)
        {

            try
            {
                decimal amount = 0;
                string phoneNumber = null;
                string transactionDesc = null;
                string accountReference = null;

                var accessToken = await GetAccessToken();
                var client = new HttpClient();
                string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                var lipaNaMpesaUrl = "https://sandbox.safaricom.co.ke/mpesa/stkpush/v1/processrequest";

                foreach (var request in mpesapayload.Mpesa)
                {
                    amount = request.Amount;
                    phoneNumber = request.PhoneNumber;
                    transactionDesc = request.TransactionDesc;
                    accountReference = request.AccountReference;
                }

                string password = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{LipaNaMpesaShortcode}{accessToken}{timestamp}"));

                var requestBody = new
                {
                    BusinessShortcode = "174379",
                    Password = "MTc0Mzc5YmZiMjc5ZjlhYTliZGJjZjE1OGU5N2RkNzFhNDY3Y2QyZTBjODkzMDU5YjEwZjc4ZTZiNzJhZGExZWQyYzkxOTIwMjUwMTE4MTUzODM1",
                    Timestamp = timestamp,
                    TransactionType = "CustomerPayBillOnline",
                    Amount = 1,
                    PartyA = phoneNumber,
                    PartyB = "174379",
                    PhoneNumber = phoneNumber,
                    CallBackURL = "https://mydomain.com/path",
                    AccountReference = "CompanyXLTD",
                    TransactionDesc = "Payment of X"
                };

                // Convert request body to JSON
                var jsonBody = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                // Set authorization header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await client.PostAsync(lipaNaMpesaUrl, content);
                var result = await response.Content.ReadAsStringAsync();

                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = result
                };

            }
            catch (Exception ex)
            {
                return new ResponseStatus
                {
                    Status = 500,
                    StatusMessage = "Internal Server Error: " + ex.Message
                };
            }
        }


        public async Task<string> GetAccessToken()
        {


            using var client = new HttpClient();

            //     // Set the URL
            string tokenUri = "https://sandbox.safaricom.co.ke/oauth/v1/generate?grant_type=client_credentials";


            var byteArray = Encoding.ASCII.GetBytes($"{MpesaConsumerKey}:{MpesaConsumerSecret}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            try
            {
                // Send GET request
                HttpResponseMessage response = await client.GetAsync(tokenUri);
                response.EnsureSuccessStatusCode(); // Throws exception if the status code is not 2xx

                // Read the response
                var responseData = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseData);

                var outerJson = System.Text.Json.Nodes.JsonNode.Parse(responseData);
                var embeddedJsonString = outerJson?["access_token"]?.ToString();

                return embeddedJsonString;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }



        }


        public async Task<ResponseStatus> RegisterUrls(RegisterUrlsRequest registerUrlsRequest)
        {

            try
            {
                var _httpClient = new HttpClient();
                var accessToken = await GetAccessToken();
                var RegisterApisUrls = "https://sandbox.safaricom.co.ke/mpesa/c2b/v1/registerurl";

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.PostAsJsonAsync(RegisterApisUrls, registerUrlsRequest);

                if (response.IsSuccessStatusCode)
                {// Success
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return new ResponseStatus
                    {
                        Status = 200,
                        StatusMessage = responseBody
                    };
                }
                return new ResponseStatus
                {
                    Status = (int)response.StatusCode,
                    StatusMessage = await response.Content.ReadAsStringAsync()
                };

            }
            catch (Exception ex)
            {
                return new ResponseStatus
                {
                    Status = 500,
                    StatusMessage = "Internal Server Error: " + ex.Message
                };
            }

        }

        //validate the c2b transaction
        public async Task<ResponseStatus> MpesaValidaion(ValidationRequest validationrequest)
        {

            try
            {
                var _httpClient = new HttpClient();

                var validationUrl = "https://sandbox.safaricom.co.ke/mpesa/validation";


                // Send the request to M-Pesa's API
                var response = await _httpClient.PostAsJsonAsync(validationUrl, validationrequest);

                // Ensure the response is successful
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response if needed
                    var responseBody = await response.Content.ReadAsStringAsync();

                    return new ResponseStatus
                    {
                        Status = 200,
                        StatusMessage = responseBody
                    };
                }

                return new ResponseStatus
                {
                    Status = (int)response.StatusCode,
                    StatusMessage = await response.Content.ReadAsStringAsync()
                };

            }
            catch (Exception ex)
            {
                return new ResponseStatus
                {
                    Status = 500,
                    StatusMessage = "Internal Server Error: " + ex.Message
                };
            }

        }


        //simulate c2b transaction
        public async Task<ResponseStatus> MpesaC2B_Transaction(SimulateC2BRequest request)
        {
            try
            {
                var mpesaUrl = "https://sandbox.safaricom.co.ke/mpesa/c2b/v1/simulate"; // Use production URL for live

                var accessToken = await GetAccessToken();
                var _httpClient = new HttpClient();


                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.PostAsJsonAsync(mpesaUrl, request);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    //return Ok(responseBody); // Success

                    return new ResponseStatus
                    {
                        Status = 200,
                        StatusMessage = responseBody
                    };
                }
                return new ResponseStatus
                {
                    Status = (int)response.StatusCode,
                    StatusMessage = await response.Content.ReadAsStringAsync()
                };

            }
            catch (Exception ex)
            {
                return new ResponseStatus
                {
                    Status = 500,
                    StatusMessage = "Internal Server Error: " + ex.Message
                };
            }

        }


    }

}