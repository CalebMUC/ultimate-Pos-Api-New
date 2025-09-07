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

        public async Task<IEnumerable<GetTillDto>> GetTillAsync() {
            try
            {
                var response = await _dbContext.tills.Select(t => new GetTillDto
                {
                    TillId = t.TillId,
                    Name = t.Name,
                    Description = t.Description,
                    Status = t.Status,
                    OpeningAmount = t.OpeningAmount,
                    CurrentAmount = t.CurrentAmount,
                    ExpectedAmount = t.ExpectedAmount,
                    CreatedAt = t.CreatedAt,
                    CreatedBy = t.CreatedBy,
                    OpenedAt = t.OpenedAt,
                    OpenedBy = t.OpenedBy,
                    ClosingAmount = t.ClosingAmount,
                    UpdatedAt = t.UpdatedAt,
                    UpdatedBy = t.UpdatedBy,
                    SupervisedBy = t.SupervisedBy,
                    SupervisedOn = t.SupervisedOn,
                    Variance = t.Variance

                }).ToListAsync();

                return response;
            }
            catch (Exception ex) { 
                _logger.LogError("Error Fetching Tills", ex);
                return new List<GetTillDto>();

            }
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

        public async Task<ResponseStatus> UpdateTillAsync(UpdateTillDto updateTillDto)
        {
            try
            {
                var till = await _dbContext.tills.FindAsync(updateTillDto.TillId);
                if (till == null)
                {
                    return new ResponseStatus
                    {
                        Status = 400,
                        StatusMessage = "Till with the Provided Id Doesnt Exist"
                    };
                }
                updateTillFromDto(updateTillDto, till);
                await _dbContext.SaveChangesAsync();
                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Till Updated SuccessFully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Updating Till", ex);
                return new ResponseStatus
                {
                    Status = 500,
                    StatusMessage = ex.Message
                };
            }
        }

        public async Task<ResponseStatus> DeleteTillAsync(int tillId)
        {
            try
            {
                var till = await _dbContext.tills.FindAsync(tillId);
                if (till == null || till.IsDeleted)
                {
                    return new ResponseStatus
                    {
                        Status = 400,
                        StatusMessage = "Till with the provided Id doesn't exist or is already deleted."
                    };
                }

                // Soft delete (mark as deleted instead of removing)
                till.IsDeleted = true;
                till.UpdatedAt = DateTime.UtcNow;

                _dbContext.tills.Update(till);
                await _dbContext.SaveChangesAsync();

                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Till soft-deleted successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error soft-deleting Till");
                return new ResponseStatus
                {
                    Status = 500,
                    StatusMessage = ex.Message
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
                        StatusMessage = "Till with the Provided Id Doesnt Exist"
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

        public void updateTillFromDto(UpdateTillDto dto, Till till) {
            till.Name = dto.Name;
            till.Description = dto.Description;
            till.Status = dto.Status;
            till.UpdatedBy = dto.UpdatedBy;
            till.UpdatedAt = dto.UpdatedAt;




        }


    }
}
