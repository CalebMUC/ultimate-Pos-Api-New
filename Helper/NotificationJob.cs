using Quartz;
using Ultimate_POS_Api.DTOS;
using System;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ultimate_POS_Api.Repository;
using Ultimate_POS_Api.Models;
using System.Text.Json;

namespace Ultimate_POS_Api.Helper
{
   public class NotificationJob : IJob
   {
      private readonly ILogger<NotificationJob> _logger;
      private readonly INotificationRepository _notificationRepository;
      private readonly FluxApiSettings _fluxApiSettings;
      private readonly IHttpClientFactory _httpClientFactory;

      public NotificationJob(
         ILogger<NotificationJob> logger,
         IHttpClientFactory httpClientFactory,
         INotificationRepository notificationRepository,
         IOptions<FluxApiSettings> fluxApiOptions)
      {
         _logger = logger;
         _httpClientFactory = httpClientFactory;
         _notificationRepository = notificationRepository;
         _fluxApiSettings = fluxApiOptions.Value;
      }

      public async Task Execute(IJobExecutionContext context)
      {
         _logger.LogInformation($"NotificationJob started at {DateTime.Now}");

         try
         {
            var dataToSend = await _notificationRepository.PendingNotifications();

            if (dataToSend != null && dataToSend.Any())
            {
               _logger.LogInformation($"Found {dataToSend.Count()} items to send. Calling external API...");

               foreach (var item in dataToSend)
               {
                  var success = await SendSmsApiAsync(item);
                  if (success)
                  {
                     _logger.LogInformation($"Successfully sent item {item} to API.");

                  }
                  else
                  {
                     _logger.LogError($"Failed to send item {item} to API.");
                     // Handle failure: log, retry later, move to error queue, etc.
                  }
               }
               _logger.LogInformation("Finished processing data for API call.");
            }
            else
            {
               _logger.LogInformation("No pending data to send. Skipping API call.");
            }
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, "Error occurred in NotificationServiceJob.");
            // Log the exception, send alert, etc.
         }

         _logger.LogInformation($"NotificationServiceJob finished at {DateTime.Now}");
      }

      private async Task<bool> SendSmsApiAsync(Notification item)
      {
         string smsApiEndpoint = _fluxApiSettings.ApiUrl;//?? "/api/sendsms";
         try
         {

            var httpClient = _httpClientFactory.CreateClient("FluxApi");
            var token = await GetAccessToken();
            if (string.IsNullOrEmpty(token))
            {
               _logger.LogError("Failed to get dynamic token for external API call.");
               return false;
            }


            var request = new HttpRequestMessage(HttpMethod.Post, smsApiEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jsonContent = System.Text.Json.JsonSerializer.Serialize(item.SmsRequest);
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
               _logger.LogInformation($"Successfully sent item {item.MessageID} to API. Status: {response.StatusCode}");
               return true;
            }
            else
            {
               var errorContent = await response.Content.ReadAsStringAsync();
               _logger.LogError($"Failed to send item {item.MessageID} to API. Status: {response.StatusCode}, Response: {errorContent}");
               return false;
            }
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, $"Exception while sending item {item.MessageID} to external API from within the job.");
            return false;
         }
      }

      public async Task<string> GetAccessToken()
      {
         // Assuming FluxApiSettings contains the details for the token endpoint
         string tokenEndpoint = $"{_fluxApiSettings.ApiUrl ?? "/Token"}";
         string consumerKey = _fluxApiSettings.ConsumerKey;
         string consumerSecret = _fluxApiSettings.ConsumerSecret;

         var payload = new Dictionary<string, string>
         {
             {"ConsumerKey", consumerKey},
             {"ConsumerSecret", consumerSecret}
         };


         var authClient = _httpClientFactory.CreateClient("AuthClient");

         var jsonPayload = System.Text.Json.JsonSerializer.Serialize(payload);
         var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

         try
         {
            var response = await authClient.PostAsync(tokenEndpoint, content);
            if (!response.IsSuccessStatusCode)
            {
               _logger.LogError($"Token request failed with status code {response.StatusCode}. Response: {await response.Content.ReadAsStringAsync()}");
               return string.Empty;
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            var responseJson = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);

            if (responseJson != null && responseJson.TryGetValue("token", out var token))
            {
               return token.ToString()!;
            }
            else
            {
               _logger.LogError("Failed to retrieve 'token' from the API response.");
               return string.Empty;
            }
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, "Exception while trying to get access token.");
            return string.Empty;
         }
      }
   }
}