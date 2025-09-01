using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;
using Ultimate_POS_Api.Repository;
using OfficeOpenXml;
using Microsoft.AspNetCore.Authorization;


namespace Ultimate_POS_Api.Controllers
{
   [ApiController]
   [Route("api/[controller]")]
   public class AccountController : ControllerBase
   {

      private readonly IAccountRepository _accountrepository;

      private readonly ICommonRepository _commonRepository;

      public AccountController(IAccountRepository accountrepository, ICommonRepository commonRepository)
      {
         _accountrepository = accountrepository;
         _commonRepository = commonRepository;
      }

      [HttpPost("AddAccount")]
      [Authorize]
      public async Task<ActionResult> AddAccount(AccountDto accountDto)
      {
         try
         {
            var response = await _accountrepository.AddAccount(accountDto);

            string strRequest = JsonConvert.SerializeObject(accountDto);
            string strResponse = JsonConvert.SerializeObject(response);
            LogsDto log = new LogsDto()
            {
               Module = "Add account",
               Request = strRequest,
               Status = response.Status.ToString(),
               Response = strResponse
            };

            await _commonRepository.AddSystemLogs(log);


            return Ok(response);
         }
         catch (Exception ex)
         {
            return BadRequest(ex.Message);
         }

      }

      [HttpPost("GetAccounts")]
      [Authorize]
      public async Task<IActionResult> GetAccounts([FromQuery] string? searchTerm)
      {

         var response = await _accountrepository.GetAccounts();
         string strRequest = ""; //JsonConvert.SerializeObject(searchTerm);
         string strResponse = "";

         if (searchTerm == null)
         {

            strRequest = JsonConvert.SerializeObject(searchTerm);
            strResponse = JsonConvert.SerializeObject(response);
            LogsDto logs = new LogsDto()
            {
               Module = "Get account",
               Request = strRequest,
               Status = (response != null) ? "200" : "503",
               Response = strResponse
            };
            await _commonRepository.AddSystemLogs(logs);

            return Ok(response);
         }
         var filteredUsers = response
             .Where(u => u.AccountId.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
             .ToList();

         strRequest = JsonConvert.SerializeObject(searchTerm);
         strResponse = JsonConvert.SerializeObject(response);
         LogsDto log = new LogsDto()
         {
            Module = "Get account",
            Request = strRequest,
            Status = (filteredUsers != null) ? "200" : "503",
            Response = strResponse
         };

         await _commonRepository.AddSystemLogs(log);

         return Ok(response);
      }

      [HttpPost("UpdateAccount")]
      [Authorize]
      public async Task<ActionResult> UpdateAccount(UpdateAccountDTO accountDto)
      {
         try
         {
            var response = await _accountrepository.UpdateAccount(accountDto);

            string strRequest = JsonConvert.SerializeObject(accountDto);
            string strResponse = JsonConvert.SerializeObject(response);
            LogsDto log = new LogsDto()
            {
               Module = "Update account",
               Request = strRequest,
               Status = (response != null) ? "200" : "503",
               Response = strResponse
            };

            // await _commonRepository.AddSystemLogs(log);
            return Ok(response);
         }
         catch (Exception ex)
         {
            return BadRequest(ex.Message);
         }
      }

      [HttpPost("OpenCloseAccount")]
      [Authorize]
      public async Task<ActionResult> OpenCloseAccount(OpenCloseAccountDto accountDto)
      {
         try
         {
            var response = await _accountrepository.OpenCloseAccount(accountDto);

            string strRequest = JsonConvert.SerializeObject(accountDto);
            string strResponse = JsonConvert.SerializeObject(response);
            LogsDto log = new LogsDto()
            {
               Module = "OpenCloseAccount",
               Request = strRequest,
               Status = (response != null) ? "200" : "503",
               Response = strResponse
            };
            await _commonRepository.AddSystemLogs(log);
            return Ok(response);
         }
         catch (Exception ex)
         {
            return BadRequest(ex.Message);
         }
      }

   }
}