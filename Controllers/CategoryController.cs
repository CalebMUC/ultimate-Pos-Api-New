using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;
using Ultimate_POS_Api.Repository;


namespace Ultimate_POS_Api.Controllers
{
   [ApiController]
   [Route("api/[controller]")]
   public class CategoryController : ControllerBase
   {
      private readonly ICategoryRepository _categoryRepository;

      public CategoryController(ICategoryRepository categoryRepository)
      {
         _categoryRepository = categoryRepository;

      }
      [HttpPost("AddCategory")]
      [Authorize]
      public async Task<ActionResult> AddCategory(CategoryDTOs category)
      {
         try
         {
            var response = await _categoryRepository.AddCategory(category);
            return Ok(response);
         }
         catch (Exception ex)
         {
            return BadRequest(ex.Message);
         }


      }

        [HttpPost("EditCategory")]
        [Authorize]
        public async Task<ActionResult> EditCategory(EditCategoryDTO category)
        {
            try
            {
                var response = await _categoryRepository.EditCategoriesAsync(category);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpPost("GetCategory")]
      [Authorize]
      public async Task<ActionResult> GetCategory()
      {
         try
         {
            var response = await _categoryRepository.GetCategory();
            return Ok(response);
         }
         catch (Exception ex) 
         {
            return BadRequest(ex.Message);
         }

      }
   }
}
