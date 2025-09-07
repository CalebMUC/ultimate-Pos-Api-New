using Microsoft.EntityFrameworkCore;
using Ultimate_POS_Api.Data;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.DTOS.Till;
using Ultimate_POS_Api.Models;

namespace Ultimate_POS_Api.Repository.TillRepo
{
    public class TillRepository : ITillRepository
    {
        private readonly UltimateDBContext _dbContext;
        private readonly ILogger<TillRepository> _logger;
        public TillRepository(UltimateDBContext dbContext, ILogger<TillRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ResponseStatus> AddTill(AddTillDto addTill)
        {
            try
            {
                var newTill = new Till
                {
                    Name = addTill.Name,
                    Description = addTill.Description,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = addTill.CreatedBy,
                    Status = "New",   // Or "Created"
                };

                _dbContext.tills.Add(newTill);
                await _dbContext.SaveChangesAsync();

                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Till created successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Creating Till", ex);

                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = $"Error creating till: {ex.Message}"
                };
            }
        }

        public async Task<ResponseStatus> OpenTillAsync(OpenTillDto openTillDto)
        {
            try
            {
                var till = await _dbContext.tills.FindAsync(openTillDto.TillId);
                if (till == null) {
                    return new ResponseStatus
                    {
                        Status = 400,
                        StatusMessage = "Till with the Provided Id Dont Exist"
                    };
                }

                till.OpeningAmount = openTillDto.OpeningAmount;
                till.CurrentAmount = openTillDto.OpeningAmount;
                till.ExpectedAmount = openTillDto.ExpectedAmount;
                till.Status = "Open";
                till.OpenedBy = openTillDto.OpenedBy;
                till.OpenedAt = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();

                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Till Opened SuccessFully"
                };
            }
            catch (Exception ex) {

                _logger.LogError("Error Opening Till", ex);
                return new ResponseStatus
                {
                    Status = 500,
                    StatusMessage = ex.Message
                };

            }
        }


    }
}
