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
                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = $"Error creating till: {ex.Message}"
                };
            }
        }


    }
}
