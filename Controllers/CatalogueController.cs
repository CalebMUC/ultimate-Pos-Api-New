using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;
using Ultimate_POS_Api.Repository;
using OfficeOpenXml;
using Microsoft.AspNetCore.Authorization;


namespace Ultimate_POS_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogueController : ControllerBase
    {

        private readonly ICatalogueRepository _catalogueRepository;
        public CatalogueController(ICatalogueRepository catalogueRepository)
        {
            _catalogueRepository = catalogueRepository;
        }

        [HttpPost("AddCatalogue")]
        [Authorize]
        public async Task<ActionResult> AddAccount(CatalogueListDTO catalogue)
        {
            try
            {
                var response = await _catalogueRepository.AddCatalog(catalogue);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("GetCatalogue")]
        [Authorize]
        public async Task<ActionResult> GetCatalogue()
        {
            try
            {
                var response = await _catalogueRepository.GetCatelogue();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetActiveCatelogue")]
        [Authorize]
        public async Task<ActionResult> GetActiveCatelogue()
        {
            try
            {
                var response = await _catalogueRepository.GetActiveCatelogue();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        // [HttpPost("upload-excel")]
        // [Authorize]
        // public async Task<IActionResult> UploadCatalogue(IFormFile file)
        // {
        //     try
        //     {

        //         if (file == null || file.Length == 0)
        //         {
        //             return BadRequest("No file uploaded or file is empty.");
        //         }

        //         var fileExtension = Path.GetExtension(file.FileName);
        //         if (fileExtension != ".xls" && fileExtension != ".xlsx")
        //         {
        //             return BadRequest("Invalid file type. Only Excel files are allowed.");
        //         }

        //         var catalogueItems = new List<CatalogueDTOs>();

        //         using (var stream = new MemoryStream())
        //         {

        //             ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        //             using (var package = new ExcelPackage())
        //             {

        //                 var worksheet = package.Workbook.Worksheets.FirstOrDefault();
        //                 if (worksheet == null)
        //                 {
        //                     return BadRequest("Excel file is empty or invalid.");
        //                 }

        //                 for (int row = 2; row <= worksheet.Dimension.Rows; row++)
        //                 {

        //                     var ProductId = Guid.Parse(worksheet.Cells[row, 1].Text);
        //                     var Name = worksheet.Cells[row, 2].Text;
        //                     var Description = worksheet.Cells[row, 3].Text;
        //                     var SKU = worksheet.Cells[row, 4].Text;
        //                     var CategoryId = worksheet.Cells[row, 5].Text;
        //                     var BuyingPrice = decimal.Parse(worksheet.Cells[row, 6].Text);
        //                     var SellingPrice = decimal.Parse(worksheet.Cells[row, 7].Text);
        //                     var Discount = decimal.Parse(worksheet.Cells[row, 8].Text);


        //                     var Availability = bool.Parse(worksheet.Cells[row, 9].Text); 

        //                     var ImageUrl = worksheet.Cells[row, 10].Text;


        //                     DateTime CreatedAt = DateTime.TryParse(worksheet.Cells[row, 11].Text, out DateTime parsedCreatedAt)
        //                                         ? parsedCreatedAt
        //                                         : DateTime.MinValue;  

        //                     DateTime UpdatedAt = DateTime.TryParse(worksheet.Cells[row, 12].Text, out DateTime parsedUpdatedAt)
        //                                         ? parsedUpdatedAt
        //                                         : DateTime.MinValue;  

        //                     // add the item to the list
        //                     catalogueItems.Add(new CatalogueDTOs
        //                     {
        //                         ProductId = ProductId,
        //                         Name = Name,
        //                         Description = Description,
        //                         SKU = SKU,
        //                         CategoryId = CategoryId,
        //                         BuyingPrice = BuyingPrice,
        //                         SellingPrice = SellingPrice,
        //                         Discount = Discount,
        //                         Availability = Availability,
        //                         ImageUrl = ImageUrl,
        //                         //CreatedAt = DateTime.UtcNow,
        //                         //UpdatedAt = DateTime.UtcNow
        //                     });
        //                 }
        //             }
        //         }


        //         var response = await _catalogueRepository.UploadCatalogue(catalogueItems);
        //         return Ok(response);


        //     }
        //     catch (Exception ex)
        //     {
        //         return BadRequest(ex.Message);
        //     }




        // }

    }

}

