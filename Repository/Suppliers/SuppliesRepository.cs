using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;
using Ultimate_POS_Api.Data;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;
using static Ultimate_POS_Api.DTOS.SuppliesDTOs;



namespace Ultimate_POS_Api.Repository
{

    public class SuppliesRepository : ISuppliesRepository
    {
        private readonly UltimateDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public SuppliesRepository(UltimateDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<ResponseStatus> AddSuplies(SuppliesDetailsDTO supplies)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            using var SuppliesScope = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                foreach (SuppliesDTOs dto in supplies.Supplies)
                {
                    // Map and add the transaction
                    Supplies newSupplies = new()
                    {

                        SupplierId = dto.SupplierId,
                        SupplyDate = dto.SuppllyDate,
                        CategoryID = dto.CategoryID,
                        ProductID = dto.ProductID,
                        Quantity = dto.Quantity,
                        CreatedOn = DateTime.UtcNow,
                        UpdatedOn = DateTime.UtcNow,
                        CreatedBy = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                        UpdatedBy = ""


                    };
                    _dbContext.Supplies.Add(newSupplies);

                }
                // Save changes to the database
                await _dbContext.SaveChangesAsync();
                await SuppliesScope.CommitAsync();

                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Supplies added successfully"
                };

            }
            catch (Exception ex)
            {

                await SuppliesScope.RollbackAsync();

                return new ResponseStatus
                {
                    Status = 500,
                    StatusMessage = $"Internal Server Error: {ex.Message}"
                };
            }
        }


        public async Task<IEnumerable<Supplies>> GetSupplies()
        {
            var response = await _dbContext.Supplies.ToListAsync();
            return response;
        }

    }

    //  public class SuppliesDetailsDTO
    //  {
    //  }
}