using Microsoft.EntityFrameworkCore;
using Ultimate_POS_Api.Data;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



namespace Ultimate_POS_Api.Repository
{
    public class CatalogueRepository : ICatalogueRepository
    {
        private readonly UltimateDBContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CatalogueRepository(UltimateDBContext dbContext, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;

        }


        public async Task<ResponseStatus> AddCatalog(CatalogueListDTO catalogueList)
        {
            try
            {
                using var CatalogueScope = await _dbContext.Database.BeginTransactionAsync();
                // var catalogueEntities = catalogueList.Cataloguee.Select(dto => new Catalogue
                // {
                //     SKU = Guid.NewGuid(),
                //     ProductId = dto.ProductId,
                //     Availability = dto.Availability,
                // }).ToList();

                foreach (var dto in catalogueList.Cataloguee)
                {


                    var newCatalogue = new Catalogue
                    {
                        SKU = Guid.NewGuid(),
                        ProductId = dto.ProductId,
                        Availability = dto.Availability,

                    };

                    // _dbContext.Catalogue.AddRange(newCatalogue);
                    _dbContext.Catalogue.Add(newCatalogue);

                    await _dbContext.SaveChangesAsync();

                    await CatalogueScope.CommitAsync();
                }

                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Catalogue Added Successfully"
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


        public async Task<IEnumerable<ProductCatalogueDTO>> GetCatelogue()
        {
            var result = await (from c in _dbContext.Catalogue
                                join p in _dbContext.Products on c.ProductId equals p.ProductID
                                select new ProductCatalogueDTO
                                {
                                    ProductId = c.ProductId,
                                    SKU = c.SKU,
                                    ProductName = p.ProductName,
                                    ProductCost = p.ProductDescription,
                                    CategoryID = p.CategoryID,
                                    Weight_Volume = p.Weight_Volume,
                                    BuyingPrice = p.BuyingPrice,
                                    SellingPrice = p.SellingPrice,
                                    Availability = c.Availability
                                }).ToListAsync();

            return result;
        }


        public async Task<IEnumerable<ProductCatalogueDTO>> GetActiveCatelogue()
        {
            var result = await (from c in _dbContext.Catalogue
                                join p in _dbContext.Products on c.ProductId equals p.ProductID
                                where c.Availability == true
                                select new ProductCatalogueDTO
                                {
                                    ProductId = c.ProductId,
                                    SKU = c.SKU,
                                    ProductName = p.ProductName,
                                    ProductCost = p.ProductDescription,
                                    CategoryID = p.CategoryID,
                                    Weight_Volume = p.Weight_Volume,
                                    BuyingPrice = p.BuyingPrice,
                                    SellingPrice = p.SellingPrice,
                                    Availability = c.Availability
                                }).ToListAsync();

            return result;
        }







        // public async Task<ResponseStatus> UploadCatalogue(List<CatalogueDTOs> catalogueDTs)
        // {
        //     try
        //     {
        //         // Map DTO to Entity and save to the database
        //         var catalogueEntities = catalogueDTs.Select(dto => new Catalogue // List<CatalogueDTOs>
        //         {
        //             ProductId = dto.ProductId,
        //             Name = dto.Name,
        //             Description = dto.Description,
        //             SKU = dto.SKU,
        //             CategoryId = dto.CategoryId,
        //             BuyingPrice = dto.BuyingPrice,
        //             SellingPrice = dto.SellingPrice,
        //             Discount = dto.Discount,
        //             Availability = dto.Availability,
        //             ImageUrl = dto.ImageUrl,
        //             CreatedAt = DateTime.UtcNow,
        //             UpdatedAt = DateTime.UtcNow
        //         }).ToList();

        //         _dbContext.Catalogue.AddRange(catalogueEntities);
        //         await _dbContext.SaveChangesAsync();

        //         return new ResponseStatus
        //         {
        //             Status = 200,
        //             StatusMessage = "Catalogue File uploaded and data saved successfully"
        //         };

        //     }
        //     catch (Exception ex)
        //     {
        //         return new ResponseStatus
        //         {
        //             Status = 500,
        //             StatusMessage = "Internal Server Error: " + ex.Message
        //         };
        //     }
        // }

    }

}
