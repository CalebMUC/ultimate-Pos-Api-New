using Microsoft.AspNetCore.Mvc;
using Ultimate_POS_Api.DTOS.Finance;
using Ultimate_POS_Api.Repository.Finance;

namespace Ultimate_POS_Api.Controllers
{
    public class FinanceController : ControllerBase
    {
        private readonly IFinanceRepo _financeRepo;
        public FinanceController(IFinanceRepo financeRepo)
        {
            _financeRepo = financeRepo;
        }

        [HttpPost("GetExpenseCategory")]
        public async Task<IActionResult> AddExpenseCategory()
        {
            try
            {
               
                var response = await _financeRepo.GetExpenseCategoryAsync();
                // Logic to add expense category goes here
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("AddExpenseCategory")]
        public async Task<IActionResult> AddExpenseCategory([FromBody] ExpenseCategoryDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("No DATA");
                }
                var response = await _financeRepo.AddExpenseCategoryAsync(dto);
                // Logic to add expense category goes here
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("EditExpenseCategory")]
        public async Task<IActionResult> EditExpenseCategory([FromBody] EditExpenseCategoryDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("No DATA");
                }
                var response = await _financeRepo.EditExpenseCategoryAsync(dto);
                // Logic to add expense category goes here
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
