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
        public FinanceRepo(ILogger<FinanceRepo> logger, UltimateDBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<GetExpenseCategory>> GetExpenseCategoryAsync()
        {
            try
            {

                var categories = await _dbContext.expensecategories
                    .Select(c => new GetExpenseCategory
                    {
                        expensecategoryid = c.expensecategoryid,
                        name = c.name,
                        description = c.description,
                        status = c.isactive ? "Active" : "Inactive",
                        createdby = c.createdby,
                        createdon = c.createdon,
                        updatedby = c.updatedby,
                        updatedon = c.updatedon
                    })
                    .ToListAsync();

                return categories;


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching expense categories");
                return [];
            }
        }
        public async Task<ResponseStatus> AddExpenseCategoryAsync(ExpenseCategoryDto dto)
        {
            try
            {
                //check if the Category Exists
                var existingCategory = await _dbContext.expensecategories
                    .FirstOrDefaultAsync(c => c.name.ToLower() == dto.Name.ToLower());
                if (existingCategory != null)
                {
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
                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Expense category added successfully"
                };


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding expense category");
                return new ResponseStatus { Status = 500, StatusMessage = ex.Message };

            }

        }
        public async Task<ResponseStatus> EditExpenseCategoryAsync(EditExpenseCategoryDto dto)
        {
            try
            {

                var existingEntity = await _dbContext.expensecategories
                    .FindAsync(dto.expenseCategoryId);

                if (existingEntity == null)
                {
                    return new ResponseStatus { Status = 400, StatusMessage = "ExpenseId with the provided id Doesnt exist" };
                }

                //update the entity
                UpdateFromDto(existingEntity, dto);
                await _dbContext.SaveChangesAsync();

                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Expense category updated successfully"
                };


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing expense category");
                return new ResponseStatus { Status = 500, StatusMessage = ex.Message };
            }
        }

        public async Task<ResponseStatus> DeleteExpenseCategoryAsync(Guid expenseId)
        {
            try
            {
                var existingEntity = await _dbContext.expensecategories
                    .FindAsync(expenseId);
                if (existingEntity == null)
                {
                    return new ResponseStatus { Status = 400, StatusMessage = "ExpenseId with the provided id Doesnt exist" };
                }
                //delete the entity
                _dbContext.expensecategories.Remove(existingEntity);
                await _dbContext.SaveChangesAsync();
                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Expense category deleted successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting expense category");
                return new ResponseStatus { Status = 500, StatusMessage = ex.Message };
            }
        }

        public void UpdateFromDto(ExpenseCategory entity, EditExpenseCategoryDto dto)
        {
            entity.description = dto.Description;
            entity.isactive = dto.Status == "Active" ? true : false;
            entity.name = dto.Name;
            entity.updatedby = dto.UpdatedBy;
            entity.updatedon = DateTime.UtcNow;

        }
        //--------------------------------------------------------------------------
        //Expenses
        // --------------------------------------------------------------------------

        public async Task<IEnumerable<GetExpensesDto>> GetExpensesAsync()
        {
            try
            {
                var expenses = await _dbContext.Expenses
                    .Include(e => e.expenseCategory)
                    .Select(e => new GetExpensesDto
                    {
                        expenseId = e.expenseId,
                        Name = e.Name,
                        Description = e.Description,
                        Amount = e.Amount,
                        expenseCategoryId = e.expenseCategory.expensecategoryid,
                        ExpenseCategoryName = e.expenseCategory.name,
                        ExpenseDate = e.ExpenseDate,
                        isActive = e.isActive,
                        ReferenceNo = e.ReferenceNo,
                        CreatedBy = e.CreatedBy,
                        CreatedOn = e.CreatedOn,
                        UpdatedBy = e.UpdatedBy,
                        UpdatedOn = e.UpdatedOn
                    })
                    .ToListAsync();
                return expenses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching expenses");
                return [];
            }
        }

        public async Task<ResponseStatus> AddExpenseAsync(ExpenseDto dto)
        {
            try
            {
                var expense = _dbContext.Expenses
                    .FirstOrDefault(e => e.Name.ToLower() == dto.Name.ToLower());
                if (expense != null)
                {
                    return new ResponseStatus { Status = 409, StatusMessage = "Expense with the same name already exists" };
                }

                var newExpense = new Expense
                {
                    expenseId = Guid.NewGuid(),
                    Name = dto.Name,
                    Description = dto.Description,
                    Amount = dto.Amount,
                    expenseCategoryId = dto.expenseCategoryId,
                    ExpenseDate = dto.ExpenseDate,
                    isActive = dto.isActive,
                    ReferenceNo = dto.ReferenceNo,
                    CreatedBy = dto.CreatedBy,
                    CreatedOn = DateTime.UtcNow
                };
                await _dbContext.Expenses.AddAsync(newExpense);
                await _dbContext.SaveChangesAsync();
                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Expense added successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding expense category");
                return new ResponseStatus { Status = 500, StatusMessage = ex.Message };
            }




        }

        public async Task<ResponseStatus> EditExpenseAsync(EditExpenseDto dto)
        {
            try
            {
                var existingEntity = await _dbContext.Expenses
                    .FindAsync(dto.expenseId);
                if (existingEntity == null)
                {
                    return new ResponseStatus { Status = 400, StatusMessage = "Expense with the provided id Doesnt exist" };
                }
                //update the entity
                UpdateFromDto(existingEntity, dto);
                await _dbContext.SaveChangesAsync();
                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Expense updated successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing expense category");
                return new ResponseStatus { Status = 500, StatusMessage = ex.Message };
            }
        }

        public async Task<ResponseStatus> DeleteExpenseAsync(Guid expenseId)
        {
            try
            {
                var existingEntity = await _dbContext.Expenses
                    .FindAsync(expenseId);
                if (existingEntity == null)
                {
                    return new ResponseStatus { Status = 400, StatusMessage = "Expense with the provided id Doesnt exist" };
                }
                //delete the entity
                _dbContext.Expenses.Remove(existingEntity);
                await _dbContext.SaveChangesAsync();
                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Expense deleted successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting expense");
                return new ResponseStatus { Status = 500, StatusMessage = ex.Message };
            }
        }

        public void UpdateFromDto(Expense entity, EditExpenseDto dto)
        {
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.Amount = dto.Amount;
            entity.expenseCategoryId = dto.expenseCategoryId;
            entity.ExpenseDate = dto.ExpenseDate;
            entity.isActive = dto.isActive;
            entity.ReferenceNo = dto.ReferenceNo;
            entity.UpdatedBy = dto.UpdatedBy;
            entity.UpdatedOn = DateTime.UtcNow;
        }
    }
}
