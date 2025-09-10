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

        public async Task<IEnumerable<GetTillDto>> GetTillAsync()
        {
            try
            {
                var response = await (
                    from t in _dbContext.tills
                    join u in _dbContext.Users on t.UserId equals u.UserId
                    select new GetTillDto
                    {
                        TillId = t.TillId,
                        Name = t.Name,
                        UserId = t.UserId,
                        CashierName = u.UserName, // 🟢 Add cashier username
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
                    }
                ).ToListAsync();

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Fetching Tills");
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
                    UserId = addTill.UserId
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
        public async Task<ResponseStatus> CloseTillAsync(CloseTillDto closeTillDto)
        {
            try
            {
                var till = await _dbContext.tills.FindAsync(closeTillDto.TillId);
                if (till == null)
                {
                    return new ResponseStatus
                    {
                        Status = 400,
                        StatusMessage = "Till with the Provided Id Doesnt Exist"
                    };
                }
                till.ClosingAmount = closeTillDto.ClosingAmount;
                till.CurrentAmount = closeTillDto.ClosingAmount;
                till.Variance = closeTillDto.ClosingAmount - till.ExpectedAmount;
                till.Status = "Closed";
                till.UpdatedBy = closeTillDto.ClosedBy;
                await _dbContext.SaveChangesAsync();
                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Till Closed SuccessFully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Closing Till", ex);
                return new ResponseStatus
                {
                    Status = 500,
                    StatusMessage = ex.Message
                };
            }
        }

        public async Task<ResponseStatus> AssignTillAsync(AssignTillDto assignTillDto)
        {
            try
            {
                var till = await _dbContext.tills.FindAsync(assignTillDto.TillId);
                if (till == null)
                {
                    return new ResponseStatus
                    {
                        Status = 400,
                        StatusMessage = "Till with the Provided Id Doesnt Exist"
                    };
                }
                till.UserId = assignTillDto.UserId;
                till.UpdatedBy = assignTillDto.AssignedBy;
                till.UpdatedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Till Assigned SuccessFully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Assigning Till", ex);
                return new ResponseStatus
                {
                    Status = 500,
                    StatusMessage = ex.Message
                };
            }
        }

        public async Task<ResponseStatus> SubmitTillClosureAsync(TillClosureDto dto)
        {
            var till = await _dbContext.tills.FindAsync(dto.TillId);
            if (till == null) return new ResponseStatus { Status = 404, StatusMessage = "Till not found" };

            if (till.Status != "Open")
                return new ResponseStatus { Status = 400, StatusMessage = "Till must be open to close" };


            till.ClosingAmount = dto.ClosingAmount;
            till.CurrentAmount = dto.ClosingAmount;
            //till.Variance = dto.ClosingAmount - till.ExpectedAmount;
            till.Variance = dto.Variance;
            till.Status = "PendingSupervision";
            till.UpdatedBy = dto.ClosedBy;
            till.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return new ResponseStatus { Status = 200, StatusMessage = "Till sent for supervision" };
        }

        // Supervisor gets tills under review
        public async Task<IEnumerable<GetTillDto>> GetTillsUnderReviewAsync()
        {
            return await _dbContext.tills
                .Where(t => t.Status == "PendingSupervision")
                .Select(t=>new GetTillDto {
                    TillId = t.TillId,
                    Name = t.Name,
                    UserId = t.UserId,
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
                })
                .ToListAsync();
        }

        // Supervisor approves/rejects till
        public async Task<ResponseStatus> SuperviseTillAsync(ApproveTillDto dto)
        {
            var till = await _dbContext.tills.FindAsync(dto.TillId);
            if (till == null) return new ResponseStatus { Status = 404, StatusMessage = "Till not found" };

            if (till.Status != "PendingSupervision")
                return new ResponseStatus { Status = 400, StatusMessage = "Till not pending supervision" };

            if (dto.Approved)
            {
                till.Status = "Closed";
                till.SupervisedBy = dto.Supervisor;
                till.SupervisedOn = DateTime.UtcNow;
            }
            else
            {
                till.Status = "Rejected"; // You can decide business rule here
            }

            till.UpdatedBy = dto.Supervisor;
            till.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return new ResponseStatus { Status = 200, StatusMessage = $"Till {till.Status}" };
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
