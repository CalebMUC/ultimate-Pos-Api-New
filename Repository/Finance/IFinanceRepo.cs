using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.DTOS.Finance;

namespace Ultimate_POS_Api.Repository.Finance
{
    public interface IFinanceRepo
    {
        //ExpenseCategory
        public Task<IEnumerable<GetExpenseCategory>> GetExpenseCategoryAsync();
        public Task<ResponseStatus> AddExpenseCategoryAsync(ExpenseCategoryDto dto);
        public Task<ResponseStatus> EditExpenseCategoryAsync(EditExpenseCategoryDto dto);
        public Task<ResponseStatus> DeleteExpenseCategoryAsync(Guid expenseId);
        //Expense
        public Task<IEnumerable<GetExpensesDto>> GetExpensesAsync();
        public Task<ResponseStatus> AddExpenseAsync(ExpenseDto dto);
        public Task<ResponseStatus> EditExpenseAsync(EditExpenseDto dto);
        public Task<ResponseStatus> DeleteExpenseAsync(Guid expenseId);
    }
}
