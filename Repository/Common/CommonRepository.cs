using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Ultimate_POS_Api.Data;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;
using System.IO;

namespace Ultimate_POS_Api.Repository
{
   public class CommonRepository : ICommonRepository
   {
      private readonly UltimateDBContext _dbContext;
      private readonly IConfiguration _configuration;
      private readonly IHttpContextAccessor _httpContextAccessor;

      public CommonRepository(UltimateDBContext dbContext, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
      {
         _dbContext = dbContext;
         _httpContextAccessor = httpContextAccessor;
         _configuration = configuration;

      }


      public async Task AddSystemLogs(LogsDto logs)
      {
         var user = _httpContextAccessor.HttpContext?.User;
         try
         {
            var newlog = new Logs
            {
               Module = logs.Module,
               UserID = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
               Time = DateTime.UtcNow,
               Request = logs.Request,
               Status = logs.Status,
               Response = logs.Response
            };
            _dbContext.Logs.Add(newlog);

            await _dbContext.SaveChangesAsync();

            // await LogDetails(newlog.UserID, newlog.Module, newlog.Request, newlog.Response);


         }
         catch (Exception ex)
         {
            throw new Exception(ex.ToString());
         }
      }

      public async Task LogDetails(string user, string RequestName, string strParameters = "", string strResponse = "")
      {
         try
         {
            string AuditLogPath = "/Projects/Dot Net/Batch_Support"; //Convert.ToString(_appSettings.AuditLogPath);
            if (string.IsNullOrEmpty(AuditLogPath))
            {
               AuditLogPath = AppDomain.CurrentDomain.BaseDirectory + "\\AuditLog";
            }
            string AppendErrorMessage = "User ID" + ":" + user + Environment.NewLine + "Date: " + DateTime.Now + Environment.NewLine +
               "Request Name: " + RequestName + Environment.NewLine + "Parameters: " + strParameters + Environment.NewLine + "Response: " + strResponse +
               Environment.NewLine + "==================================================" + Environment.NewLine;
            string strFileName = string.Concat(AuditLogPath, "\\", user, "\\");
            Directory.CreateDirectory(strFileName);
            File.AppendAllText(strFileName + user + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage);

         }
         catch (Exception ex)
         {
            throw new Exception(ex.ToString());
         }
      }

   }
}