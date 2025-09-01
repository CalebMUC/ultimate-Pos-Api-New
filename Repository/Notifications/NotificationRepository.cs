using Microsoft.EntityFrameworkCore;
using Ultimate_POS_Api.Data;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Ultimate_POS_Api.Repository
{

   public class NotificationRepository : INotificationRepository
   {
      private readonly UltimateDBContext _dbContext;
      private readonly IHttpContextAccessor _httpContextAccessor;
      private readonly FluxApiSettings _fluxApiSettings;

      public NotificationRepository(IOptions<FluxApiSettings> fluxOptions, UltimateDBContext dbContext, IHttpContextAccessor httpContextAccessor)
      {
         _dbContext = dbContext;
         _httpContextAccessor = httpContextAccessor;
         _fluxApiSettings = fluxOptions.Value;
      }

      //Get access token 
      public async Task<string> GetAccessToken()
      {
         string FluxApiUrl = _fluxApiSettings.ApiUrl;
         string FluxApiConsumerKey = _fluxApiSettings.ApiUrl;
         string FluxApiConsumerSecret = _fluxApiSettings.ApiUrl;

         var payload = new FluxApiSettings
         {
            ConsumerKey = FluxApiConsumerKey,
            ConsumerSecret = FluxApiConsumerSecret
         };
         var fluxApiUrl = $"{FluxApiUrl}/Token";

         var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
         using var client = new HttpClient();
         try
         {
            var response = await client.PostAsync(fluxApiUrl, content);
            if (!response.IsSuccessStatusCode)
            {
               return $"Request failed with status code {response.StatusCode}.";
            }
            // Deserialize the response 
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);

            if (responseJson != null && responseJson.TryGetValue("token", out var token))
            {
               return token.ToString();
            }
            else
            {
               return "Failed to retrieve the token from the API.";
            }
         }
         catch (Exception ex)
         {
            return ex.ToString();
         }
      }

      //fetch saved contacts list
      public async Task<IEnumerable<Contacts>> GetContacts()
      {
         var response = await _dbContext.Contacts.ToListAsync();
         return response;
      }

      //Add new contacts
      public async Task<ResponseStatus> AddContact(ContactListDto contacts)
      {

         var user = _httpContextAccessor.HttpContext?.User;
         using var ContactScope = await _dbContext.Database.BeginTransactionAsync();
         try
         {
            foreach (var dto in contacts.Contact)
            {
               //check if contact exist
               var existingcontact = await _dbContext.Contacts.FirstOrDefaultAsync(p => p.PhoneNumber == dto.PhoneNumber);
               if (existingcontact != null)
               {
                  return new ResponseStatus
                  {
                     Status = 400,
                     StatusMessage = $"This phone number already exist: {existingcontact}"
                  };
               }
               else
               {
                  var newContact = new Contacts
                  {
                     ClientID = dto.ClientID,
                     Username = dto.Username,
                     PhoneNumber = dto.PhoneNumber,
                     Email = dto.Email,
                     CreatedOn = DateTime.UtcNow,
                     UpdatedOn = null,
                     CreatedBy = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                     UpdatedBy = ""
                  };
                  _dbContext.Contacts.Add(newContact);

                  await _dbContext.SaveChangesAsync();

                  await ContactScope.CommitAsync();
               }
            }

            return new ResponseStatus
            {
               Status = 200,
               StatusMessage = "Contact Added Successfully"
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

      //Add a new a notification
      public async Task<ResponseStatus> AddNotification(NotificationListDTO notificationList)
      {
         var user = _httpContextAccessor.HttpContext?.User;
         string FluxApiUsername = _fluxApiSettings.Username;
         string FluxApiSenderID = _fluxApiSettings.SenderId;

         using var transactionScope = await _dbContext.Database.BeginTransactionAsync();
         try
         {
            foreach (var dto in notificationList.Notification)
            {
               var newNotification = new Notification
               {
                  MessageID = dto.MessageID,
                  PhoneNumber = dto.PhoneNumber,
                  TextHeader = dto.TextHeader,
                  Message = dto.Message,
                  Status = dto.Status,
                  Date = DateTime.UtcNow.ToString(),//dto.Date,
                  Module = dto.Module,
                  CreatedBy = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                  SmsRequest = JsonConvert.SerializeObject(dto.SmsRequest.Select(p => new
                  {
                     Username = FluxApiUsername,
                     p.To,
                     p.Header,
                     p.Message,
                     SenderId = FluxApiSenderID,
                     Date = DateOnly.FromDateTime(DateTime.UtcNow).ToString("yyyy-MM-dd"),
                     Time = DateTime.UtcNow.TimeOfDay.ToString(@"hh\:mm\:ss")
                  }).ToList()),
               };

               _dbContext.Notification.Add(newNotification);
            }

            await _dbContext.SaveChangesAsync();

            await transactionScope.CommitAsync();

            return new ResponseStatus
            {
               Status = 200,
               StatusMessage = "Noification Added Successfully"
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

      //Add sms template
      public async Task<ResponseStatus> AddSmsTemplate(MessageTemplateListDTO TemplatePayload)
      {
         var user = _httpContextAccessor.HttpContext?.User;
         using var ContactScope = await _dbContext.Database.BeginTransactionAsync();

         try
         {
            foreach (var dto in TemplatePayload.Template)
            {
               var newTemplate = new MessageTemplates
               {
                  TemplateID = dto.TemplateID,
                  TemplateBody = dto.TemplateBody,
                  TemplateHeader = dto.TemplateHeader,
                  CreatedOn = DateTime.UtcNow.ToString(),
                  CreatedBy = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value,

               };

               _dbContext.MessageTemplates.Add(newTemplate);
            }

            await _dbContext.SaveChangesAsync();
            await ContactScope.CommitAsync();

            return new ResponseStatus
            {
               Status = 200,
               StatusMessage = "template Added Successfully"
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

      //GET all templates
      public async Task<IEnumerable<MessageTemplates>> GetSmsTemplates()
      {
         var response = await _dbContext.MessageTemplates.ToListAsync();
         return response;
      }

      //get all notifications not sent
      public async Task<IEnumerable<Notification>> PendingNotifications()
      {
         var response = await _dbContext.Notification
                                                  .Where(n => n.Status == "0")
                                                  .ToListAsync();
         return response;
      }

      //send sms method
      // public async Task<ResponseStatus> Sendsms(SmsRequestListDTO SmsPayload)
      // {

      //    if (SmsPayload.SmsPayload == null || !SmsPayload.SmsPayload.Any())
      //    {
      //       return new ResponseStatus
      //       {
      //          Status = 403,
      //          StatusMessage = "Token payload cannot be empty"
      //       };
      //    }

      //    var request = SmsPayload.SmsPayload.LastOrDefault();
      //    if (request == null)
      //    {
      //       return new ResponseStatus
      //       {
      //          Status = 403,
      //          StatusMessage = "Invalid SMS payload"
      //       };
      //    }
      //    if (string.IsNullOrWhiteSpace(apiToken) || apiToken == null)
      //    {
      //       return new ResponseStatus
      //       {
      //          Status = 403,
      //          StatusMessage = "access token is null or empty"
      //       };
      //    }

      //    try
      //    {

      //       string? username = null;
      //       string? to = null;
      //       string? header = null;
      //       string? message = null;
      //       string? date = null;
      //       string? time = null;
      //       string? token = null;

      //       // API endpoint URL

      //       //Configuration["JwtSettings:Audience"],

      //       // Create HttpClient instance and make the request
      //       using var client = new HttpClient();

      //       foreach (var dto in SmsPayload.SmsPayload)
      //       {
      //          username = dto.Username;
      //          to = dto.To;
      //          header = dto.Header;
      //          message = dto.Message;
      //          date = DateOnly.FromDateTime(DateTime.UtcNow).ToString("yyyy-MM-dd");
      //          time = DateTime.UtcNow.TimeOfDay.ToString(@"hh\:mm\:ss");
      //          token = dto.Token;
      //       }

      //       var sendsmspayload = new
      //       {
      //          username = username,
      //          to = to,
      //          header = header,
      //          message = message,
      //          date = date,
      //          time = time,
      //          token = token
      //       };

      //       // Add Authorization header
      //       client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

      //       var content = new StringContent(JsonConvert.SerializeObject(sendsmspayload), Encoding.UTF8, "application/json");

      //       var response = await client.PostAsync(SendsmsUrl, content);

      //       if (!response.IsSuccessStatusCode)
      //       {
      //          return new ResponseStatus
      //          {
      //             Status = 500,
      //             StatusMessage = "Request to send sms failed"
      //          };
      //       }

      //       var responseContent = await response.Content.ReadAsStringAsync();
      //       var apiResponse = JsonConvert.DeserializeObject<ResponseStatus>(responseContent);

      //       if (apiResponse != null)
      //       {
      //          return new ResponseStatus
      //          {
      //             Status = apiResponse.Status,
      //             StatusMessage = apiResponse.StatusMessage
      //          };
      //       }
      //       else
      //       {
      //          return new ResponseStatus
      //          {
      //             Status = 400,
      //             StatusMessage = "Failed to connection.",
      //          };
      //       }
      //    }
      //    catch (Exception ex)
      //    {
      //       return new ResponseStatus
      //       {
      //          Status = 500,
      //          StatusMessage = "Internal Server Error: " + ex.Message
      //       };
      //    }

      // }

   }

}




