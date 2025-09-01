using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json.Serialization;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;
using Ultimate_POS_Api.ReportMapping;
using Ultimate_POS_Api.Repository;

namespace Ultimate_POS_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IproductsRepository _product;
        public ProductsController(IproductsRepository product)
        {
            _product = product;
        }



        [HttpPost("AddProducts")]
        [Authorize]
        public async Task<ActionResult> AddProducts(ProductDTOs products)
        {
            try
            {
                var response = await _product.AddProducts(products);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("EditProducts")]
        [Authorize]
        public async Task<ActionResult> EditProducts(EditProductDto products)
        {
            try
            {
                var response = await _product.EditProductsAsync(products);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetProducts")]
        [Authorize]
        public async Task<ActionResult> GetProducts()
        {
            try
            {
                var response = await _product.GetProducts();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }

}