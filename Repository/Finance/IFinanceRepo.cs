using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.DTOS.Finance;

namespace Ultimate_POS_Api.Repository.Finance
{
    public interface IFinanceRepo
    {
        public Task<ResponseStatus> AddExpenseCategoryAsync(ExpenseCategoryDto dto);
    }
}
