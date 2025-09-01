

using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;

namespace Ultimate_POS_Api.Repository
{
   public interface IAccountRepository
   {

      public Task<ResponseStatus> AddAccount(AccountDto accountDto);

      public Task<IEnumerable<Accounts>> GetAccounts();

      public Task<ResponseStatus> UpdateAccount(UpdateAccountDTO accountDto);

      public Task<ResponseStatus> OpenCloseAccount(OpenCloseAccountDto accountDto);


   }
}



