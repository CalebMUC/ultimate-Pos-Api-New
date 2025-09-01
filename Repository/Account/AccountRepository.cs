using Microsoft.EntityFrameworkCore;
using Ultimate_POS_Api.Data;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;
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



namespace Ultimate_POS_Api.Repository
{
   public class AccountRepository : IAccountRepository
   {
      private readonly UltimateDBContext _dbContext;
      private readonly IConfiguration _configuration;
      private readonly IHttpContextAccessor _httpContextAccessor;


      public AccountRepository(UltimateDBContext dbContext, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
      {
         _dbContext = dbContext;
         _httpContextAccessor = httpContextAccessor;
         _configuration = configuration;

      }

      //add a new account
      public async Task<ResponseStatus> AddAccount(AccountDto accountDto)
      {
         // var user = _httpContextAccessor.HttpContext?.User;
         try
         {
            // Check if Account already exists
            var existingAccount = await _dbContext.Accounts.FirstOrDefaultAsync(u => u.UserID == accountDto.UserID);
            if (existingAccount != null)
            {
               return new ResponseStatus
               {
                  Status = 400,
                  StatusMessage = "Account Already Exists",
               };
            }
            else
            {
               var newAccount = new Accounts
               {
                  UserID = accountDto.UserID,
                  ClearBalance = 0,//accountDto.ClearBalance,
                  OpeningBalance = 0, //accountDto.OpeningBalance,
                  //ClosingBalance = 0,
                  AccountStatus = false, //accountDto.AccountStatus,
                  CreatedOn = DateTime.UtcNow,
                  UpdatedOn = DateTime.UtcNow,
                  CreatedBy = "test",//user?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                  UpdatedBy = ""
               };

               _dbContext.Accounts.Add(newAccount);


               await _dbContext.SaveChangesAsync();

               return new ResponseStatus
               {
                  Status = 200,
                  StatusMessage = "Accounts Added Successfully"
               };
            }
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

      //fetch accounts
      public async Task<IEnumerable<Accounts>> GetAccounts()
      {
         var response = await _dbContext.Accounts.ToListAsync();
         return response;
      }


      //update account after every transaction
      public async Task<ResponseStatus> UpdateAccount(UpdateAccountDTO accountDto)
      {
         var user = _httpContextAccessor.HttpContext?.User;
         try
         {
            var accountToUpdate = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountDto.AccountId);

            if (accountToUpdate == null)
            {
               return new ResponseStatus
               {
                  Status = 404,
                  StatusMessage = $"Account with ID '{accountDto.AccountId}' not found."
               };
            }

            if (!accountToUpdate.AccountStatus)
            {
               return new ResponseStatus
               {
                  Status = 403,
                  StatusMessage = $"Account with ID '{accountDto.AccountId}' is not active and cannot be transact."
               };
            }
            // 3. Update the account
            accountToUpdate.ClearBalance += accountDto.Amount;

            //accountToUpdate.OpeningBalance = accountDto.OpeningBalance;
            //accountToUpdate.ClosingBalance = accountDto.ClosingBalance;
            //accountToUpdate.ClearBalance = accountDto.Amount;


            accountToUpdate.UpdatedOn = DateTime.UtcNow;
            accountToUpdate.UpdatedBy = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await _dbContext.SaveChangesAsync();

            return new ResponseStatus
            {
               Status = 200,
               StatusMessage = "Account updated successfully."
            };
         }
         catch (Exception ex)
         {
            // Log the full exception details (ex.ToString()) for better debugging in a real application
            Console.WriteLine(ex.ToString()); // For development purposes
            return new ResponseStatus
            {
               Status = 500,
               StatusMessage = "Internal Server Error: " + ex.Message
            };
         }
      }


      //open and close account
      public async Task<ResponseStatus> OpenCloseAccount(OpenCloseAccountDto accountDto)
      {
         try
         {
            var user = _httpContextAccessor.HttpContext?.User;
            string updatedBy = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var accountToUpdate = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountDto.AccountId);
            if (accountToUpdate == null)
            {
               return new ResponseStatus
               {
                  Status = 404,
                  StatusMessage = $"Account with ID '{accountDto.AccountId}' not found."
               };
            }
            if (accountToUpdate.AccountStatus == accountDto.AccountStatus)
            {
               return new ResponseStatus
               {
                  Status = 403,
                  StatusMessage = $"Account status is already {(accountDto.AccountStatus ? "Open" : "Closed")}."
               };
            }

            // 4. Update the properties of the fetched Account
            accountToUpdate.AccountStatus = accountDto.AccountStatus;
            accountToUpdate.UpdatedOn = DateTime.UtcNow;
            accountToUpdate.UpdatedBy = updatedBy;

            await _dbContext.SaveChangesAsync();

            return new ResponseStatus
            {
               Status = 200,
               StatusMessage = "Account status updated successfully."
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