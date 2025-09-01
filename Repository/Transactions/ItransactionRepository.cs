using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;

namespace Ultimate_POS_Api.Repository
{
    public interface ItransactionRepository
    {
        // public Task<IEnumerable<Categories>> GetCategory();

         public Task<IEnumerable<Transactions>> GetTransactions();

        public Task<ResponseStatus> AddSale(TransactionListDto JsonData);

    }
}