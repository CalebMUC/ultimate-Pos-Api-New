using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Ultimate_POS_Api.Data;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Ultimate_POS_Api.Repository
{
   public class CategoryRepository : ICategoryRepository
   {
      private readonly UltimateDBContext _dbContext;
      private readonly IConfiguration _configuration;
      private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CategoryRepository> _logger;
             
      public CategoryRepository(UltimateDBContext dbContext,
          IHttpContextAccessor httpContextAccessor,
          IConfiguration configuration,
          ILogger<CategoryRepository> logger
          )
      {
         _dbContext = dbContext;
         _httpContextAccessor = httpContextAccessor;
         _configuration = configuration;
            _logger = logger;

      }


      public async Task<ResponseStatus> AddCategory(CategoryDTOs categorydto)
      {
         var user = _httpContextAccessor.HttpContext?.User;

         var username = user?.FindFirst(ClaimTypes.Name)?.Value;
         var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
         var role = user?.FindFirst(ClaimTypes.Role)?.Value;

         // Check if CATEGORY already exists
         var existingCategory = await _dbContext.Categories.FirstOrDefaultAsync(u => u.CategoryName == categorydto.CategoryName || u.CategoryCode == categorydto.CategoryCode);
         if (existingCategory != null)
         {
            return new ResponseStatus
            {
               Status = 400,
               StatusMessage = "Category Already Exists",
            };
         }


         var newcategory = new Categories
         {
            CategoryName = categorydto.CategoryName,
            CategoryCode = categorydto.CategoryCode,
            CategoryDescription = categorydto.CategoryDescription,
            NoOfItems = categorydto.NoOfItems,
            Status = categorydto.Status,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            CreatedBy = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value,


            //CreatedBy = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value,

            UpdatedBy = ""//user?.FindFirst(ClaimTypes.NameIdentifier)?.Value,  UserId

         };

         // Add the new category to the database
         //_dbContext.Categories.Add(newcategory);

         _dbContext.Categories.Add(newcategory);

         try
         {
            // Save changes to the database asynchronously
            await _dbContext.SaveChangesAsync();

            return new ResponseStatus
            {
               Status = 200,
               StatusMessage = "Category Added Successfully"
            };
         }
         catch (Exception ex)
         {
            return new ResponseStatus
            {
               Status = 500,
               StatusMessage = "Internal Server Error: " + ex.Message
            };
         }


      }
        public async Task<ResponseStatus> EditCategoriesAsync(EditCategoryDTO editCategoryDTO)
        {
            try
            {
                //get Category based on CategoruID
                var existingCategory = await _dbContext.Categories.FindAsync(editCategoryDTO.CategoryID);
                if(existingCategory != null)
                {
                    UpdateEntityFromDto(existingCategory, editCategoryDTO);
                    await _dbContext.SaveChangesAsync();
                }

                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Category Edited Successfully"
                };
            }
            catch (Exception ex) {
                _logger.LogError($"Error : {ex.Message}");
                return new ResponseStatus
                {
                    Status = 500,
                    StatusMessage = "Internal Server error"
                };
            }
        }

        public void UpdateEntityFromDto(Categories entity, EditCategoryDTO categoryDto)
        {
            var user = _httpContextAccessor.HttpContext?.User;
         
                entity.CategoryID = categoryDto.CategoryID;
                entity.CategoryName = categoryDto.CategoryName;
                entity.CategoryCode = categoryDto.CategoryCode;
                entity.CategoryDescription = categoryDto.CategoryDescription;
                entity.NoOfItems = categoryDto.NoOfItems;
                entity.Status = categoryDto.Status;
                entity.UpdatedOn = DateTime.UtcNow;
                entity.UpdatedBy = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
          
        }

      public async Task<IEnumerable<Categories>> GetCategory()
      {
         var response = await _dbContext.Categories.ToListAsync();
         return response;
      }
   }
}


