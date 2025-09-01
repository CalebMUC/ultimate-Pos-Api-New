using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;
using Ultimate_POS_Api.Repository;

namespace Ultimate_POS_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly ISuppliersRepository _supplierrepository;
        public SuppliersController(ISuppliersRepository supplierS)
        {
            _supplierrepository = supplierS;
        }

        [HttpPost("AddSupplier")]
        [Authorize]
        public async Task<IActionResult> AddSuplier(SuppliersDetailsDTO supplier)
        {

            if (supplier == null)
            {
                return BadRequest("No DATA");
            }
            try
            {
                var response = await _supplierrepository.AddSuplier(supplier);
                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("EditSupplier")]
        [Authorize]
        public async Task<IActionResult> EditSuplier(SuppliersDetailsDTO supplier)
        {

            if (supplier == null)
            {
                return BadRequest("No DATA");
            }
            try
            {
                var response = await _supplierrepository.EditSuplierAsync(supplier);
                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [HttpPost("GetSuppliers")]
        [Authorize]
        public async Task<ActionResult> GetSupplier()
        {


            try
            {

                var response = await _supplierrepository.GetSupplier();

                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }





    }
}
