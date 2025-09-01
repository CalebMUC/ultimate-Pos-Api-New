using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;

namespace Ultimate_POS_Api.Repository
{
   public interface ICommonRepository
   {
      public Task AddSystemLogs(LogsDto logs);

      public Task LogDetails(string user, string RequestName, string strParameters = "", string strResponse = "");

   }
}