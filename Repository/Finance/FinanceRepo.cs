using Microsoft.EntityFrameworkCore;
using Ultimate_POS_Api.Data;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.DTOS.Finance;
using Ultimate_POS_Api.Models;

namespace Ultimate_POS_Api.Repository.Finance
{
    public class FinanceRepo : IFinanceRepo
    {
        private readonly UltimateDBContext _dbContext;
        private ILogger<FinanceRepo> _logger;
        public FinanceRepo(ILogger<FinanceRepo> logger, UltimateDBContext dbContext) {
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task<ResponseStatus> AddExpenseCategoryAsync(ExpenseCategoryDto dto)
        {
            try
            {
                //check if the Category Exists
                var existingCategory = await _dbContext.expensecategories
                    .FirstOrDefaultAsync(c => c.name.ToLower() == dto.Name.ToLower());
                if (existingCategory != null) {
                    return new ResponseStatus { Status = 409, StatusMessage = "Expense category already exists" };
                }

                var newCategory = new ExpenseCategory
                {
                    expensecategoryid = Guid.NewGuid(),
                    name = dto.Name,
                    description = dto.Description,
                    isactive = dto.Status == "Active" ? true : false,
                    createdon = DateTime.UtcNow,
                    createdby = dto.CreatedBy

                };

                await _dbContext.expensecategories.AddAsync(newCategory);
                await _dbContext.SaveChangesAsync();
                return new ResponseStatus { Status = 200, 
                    StatusMessage = "Expense category added successfully" };


            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error adding expense category");
                return new ResponseStatus {  Status = 500, StatusMessage = ex.Message };

            }

        }
    }
}
