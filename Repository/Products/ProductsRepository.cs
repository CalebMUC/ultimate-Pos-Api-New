using Microsoft.EntityFrameworkCore;
using Ultimate_POS_Api.Data;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;
using System.Security.Claims;
using System.Data;
using System.Collections;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http.HttpResults;


namespace Ultimate_POS_Api.Repository
{
   public class ProductsRepository : IproductsRepository
   {
      private readonly UltimateDBContext _dbContext;

      private readonly IHttpContextAccessor _httpContextAccessor;

      private readonly ICatalogueRepository _catalogueRepository;

      public ProductsRepository(UltimateDBContext dbContext, ICatalogueRepository catalogueRepository, IHttpContextAccessor httpContextAccessor)
      {
         _dbContext = dbContext;
         _httpContextAccessor = httpContextAccessor;
         _catalogueRepository = catalogueRepository;
      }


      public async Task<IEnumerable<GetProductsDto>> GetProducts()
        {
            //var response = await _dbContext.Products.ToListAsync();

            var response = await _dbContext.Products
       .Join(_dbContext.Categories,
           p => p.CategoryID,
           c => c.CategoryID,
           (p, c) => new { p, c })
       .Join(_dbContext.Supplier,
           pc => pc.p.SupplierId,
           s => s.SupplierId,
           (pc, s) => new GetProductsDto
           {
               ProductID = pc.p.ProductID,
               ProductName = pc.p.ProductName,
               ProductDescription = pc.p.ProductDescription,
               Weight_Volume = pc.p.Weight_Volume,
               CategoryID = pc.c.CategoryID,
               CategoryName = pc.c.CategoryName,
               BuyingPrice = pc.p.BuyingPrice,
               SellingPrice = pc.p.SellingPrice,
               Quantity = pc.p.Quantity,
               SupplierId = s.SupplierId,
               SupplierName = s.SupplierName,
               CreatedOn = pc.p.CreatedOn,
               UpdatedOn = pc.p.UpdatedOn,
               CreatedBy = pc.p.CreatedBy,
               UpdatedBy = pc.p.UpdatedBy,
               Status = pc.p.Status
           })
       .ToListAsync();

            //var response = await _dbContext.Products
            //.Include(p => p.Supplier)
            //.ToListAsync();

            return response;

            return response;
         // throw new NotImplementedException();
      }



      // public async Task<ResponseStatus> AddProducts(ProductDTOs dto)
      // {
      //    var user = _httpContextAccessor.HttpContext?.User;

      //    try
      //    {
      //       // Check if product already exists
      //       var existingProduct = _dbContext.Products.FirstOrDefault(x => x.ProductName != null && x.ProductName == dto.ProductName);
      //       if (existingProduct != null)
      //       {
      //          return new ResponseStatus
      //          {
      //             Status = 409,
      //             StatusMessage = $"Product '{dto.ProductName}' Already Exists"
      //          };
      //       }

      //       var produdtcts = new Products
      //       {
      //          ProductName = dto.ProductName,
      //          ProductDescription = dto.ProductDescription,
      //          Weight_Volume = dto.Weight_Volume,
      //          CategoryID = dto.CategoryID,
      //          BuyingPrice = dto.BuyingPrice,
      //          SellingPrice = dto.SellingPrice,
      //          Quantity = dto.Quantity,
      //          SupplierId = dto.Supplier,

      //          //CreatedOn = dto.CreatedOn,
      //          //UpdatedOn = dto.UpdatedOn,
      //          //CreatedBy = dto.CreatedBy,
      //          //Up\datedBy = dto.UpdatedBy,

      //          CreatedOn = DateTime.UtcNow,
      //          UpdatedOn = DateTime.UtcNow,
      //          CreatedBy = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
      //          UpdatedBy = ""
      //       };

      //       _dbContext.Products.AddRange(produdtcts);
      //       //add the product
      //       await _dbContext.SaveChangesAsync();

      //       //add catalogue dependin on the no of itemss
      //       var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductName == dto.ProductName);

      //       if (product != null)
      //       {

      //          var catalogueDTOList = new List<CatalogueDTOs>();

      //          foreach (var _ in Enumerable.Range(0, product.Quantity))
      //          {
      //             var catalog = new CatalogueDTOs
      //             {
      //                //SKU = Guid.NewGuid(),
      //                ProductId = product.ProductID,
      //                Availability = true,
      //             };

      //             catalogueDTOList.Add(catalog);
      //          }

      //          CatalogueListDTO catalogueList = new CatalogueListDTO
      //          {
      //             Cataloguee = catalogueDTOList
      //          };

      //          var response = await _catalogueRepository.AddCatalog(catalogueList);


      //       }

      //       return new ResponseStatus
      //       {
      //          Status = 200,
      //          StatusMessage = "Products Added Successfully"
      //       };
      //    }
      //    catch (Exception ex)
      //    {
      //       return new ResponseStatus
      //       {
      //          Status = 500,
      //          StatusMessage = "Internal Server Error: " + ex.Message
      //       };
      //    }
      // }

      // public async Task<ResponseStatus> AddProducts(ProductDTOs dto)
      // {
      //    var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

      //    try
      //    {
      //       // Check for existing product by name (case-insensitive)
      //       var exists = await _dbContext.Products
      //           .AnyAsync(x => x.ProductName != null && x.ProductName.ToLower() == dto.ProductName.ToLower());

      //       if (exists)
      //       {
      //          return new ResponseStatus
      //          {
      //             Status = 409,
      //             StatusMessage = $"Product '{dto.ProductName}' already exists."
      //          };
      //       }

      //       // Use transaction to ensure atomicity
      //       using var transaction = await _dbContext.Database.BeginTransactionAsync();

      //       var newProduct = new Products
      //       {
      //          ProductID = Guid.NewGuid(), // Ensure the ID is generated now
      //          ProductName = dto.ProductName,
      //          ProductDescription = dto.ProductDescription,
      //          Weight_Volume = dto.Weight_Volume,
      //          CategoryID = dto.CategoryID,
      //          BuyingPrice = dto.BuyingPrice,
      //          SellingPrice = dto.SellingPrice,
      //          Quantity = dto.Quantity,
      //          SupplierId = dto.Supplier,
      //          CreatedOn = DateTime.UtcNow,
      //          UpdatedOn = DateTime.UtcNow,
      //          CreatedBy = userId,
      //          UpdatedBy = ""
      //       };

      //       _dbContext.Products.Add(newProduct);
      //       await _dbContext.SaveChangesAsync();

      //       // Create catalogue entries based on quantity
      //       var catalogueList = dto.Quantity > 0
      //           ? Enumerable.Range(0, dto.Quantity)
      //               .Select(_ => new CatalogueDTOs
      //               {
      //                  ProductId = newProduct.ProductID,
      //                  Availability = true
      //               }).ToList()
      //           : new List<CatalogueDTOs>();

      //       if (catalogueList.Count > 0)
      //       {
      //          var response = await _catalogueRepository.AddCatalog(new CatalogueListDTO
      //          {
      //             Cataloguee = catalogueList
      //          });

      //          if (response.Status != 200)
      //          {
      //             await transaction.RollbackAsync();
      //             return new ResponseStatus
      //             {
      //                Status = 500,
      //                StatusMessage = $"Failed to add catalog entries: {response.StatusMessage}"
      //             };
      //          }
      //       }

      //       await transaction.CommitAsync();

      //       return new ResponseStatus
      //       {
      //          Status = 200,
      //          StatusMessage = "Product and catalog entries added successfully."
      //       };
      //    }
      //    catch (Exception ex)
      //    {
      //       return new ResponseStatus
      //       {
      //          Status = 500,
      //          StatusMessage = $"Internal Server Error: {ex.Message}"
      //       };
      //    }
      // }


      public async Task<ResponseStatus> AddProducts(ProductDTOs dto)
      {
         var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

         try
         {
            // Check if product already exists
            var exists = await _dbContext.Products
                .AnyAsync(x => x.ProductName != null && x.ProductName.ToLower() == dto.ProductName.ToLower());

            if (exists)
            {
               return new ResponseStatus
               {
                  Status = 409,
                  StatusMessage = $"Product '{dto.ProductName}' already exists."
               };
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            // Create product
            var newProduct = new Products
            {
               ProductID = Guid.NewGuid(),
               ProductName = dto.ProductName,
               ProductDescription = dto.ProductDescription,
               Weight_Volume = dto.Weight_Volume,
               CategoryID = dto.CategoryID,
               BuyingPrice = dto.BuyingPrice,
               SellingPrice = dto.SellingPrice,
               Quantity = dto.Quantity, 
               SupplierId = dto.Supplier,
               CreatedOn = DateTime.UtcNow,
               UpdatedOn = DateTime.UtcNow,
               CreatedBy = userId,
               UpdatedBy = "",
               Status = true
            };

            _dbContext.Products.Add(newProduct);
            await _dbContext.SaveChangesAsync();

            // Create catalogue items
            var catalogueEntities = Enumerable.Range(0, newProduct.Quantity)
                .Select(_ => new Catalogue
                {
                   SKU = Guid.NewGuid(),
                   ProductId = newProduct.ProductID,
                   Availability = true
                }).ToList();

            _dbContext.Catalogue.AddRange(catalogueEntities);
            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return new ResponseStatus
            {
               Status = 200,
               StatusMessage = "Product and catalogue added successfully"
            };
         }
         catch (Exception ex)
         {
            return new ResponseStatus
            {
               Status = 500,
               StatusMessage = $"Internal Server Error: {ex.Message}"
            };
            }
        }


        public async Task<ResponseStatus> EditProductsAsync(EditProductDto products)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var existingProduct = await _dbContext.Products.FindAsync(products.ProductID);
                 UpdateProductsFromDto(existingProduct, products,userId);
                await _dbContext.SaveChangesAsync();

                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Products Edit Successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResponseStatus
                {
                    Status = 500,
                    StatusMessage = $"Products failed to  Edited {ex.Message} "
                };

            }
        }

        public void UpdateProductsFromDto(Products entity, EditProductDto productDto,string userId)
        {
            entity.ProductName = productDto.ProductName;
            entity.ProductDescription = productDto.ProductDescription;
            entity.Weight_Volume = productDto.Weight_Volume;
            entity.CategoryID = productDto.CategoryID;
            entity.BuyingPrice = productDto.BuyingPrice;
            entity.SellingPrice = productDto.SellingPrice;
            entity.Quantity = productDto.Quantity;
            entity.SupplierId = productDto.Supplier;
            entity.UpdatedOn = DateTime.UtcNow;
            entity.UpdatedBy = userId;
            entity.Status = true;
        }


   }

}

