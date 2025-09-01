using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Repository;
// using static Ultimate_POS_Api.DTOS.SuppliesDTOs;

namespace Ultimate_POS_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliesController : ControllerBase
    {
        private readonly ISuppliesRepository _suppliesrepository;
        public SuppliesController(ISuppliesRepository supplies)
        {
            _suppliesrepository = supplies;
        }

        [HttpPost("AddSupplies")]
        [Authorize]
        public async Task<IActionResult> AddSuplies(SuppliesDetailsDTO supplies)
        {

            if (supplies == null)
            {
                return BadRequest("No DATA");
            }
            try
            {
                var response = await _suppliesrepository.AddSuplies(supplies);
                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [HttpPost("GetSupplies")]
        [Authorize]
        public async Task<ActionResult> GetSupplies()
        {
            try
            {

                var response = await _suppliesrepository.GetSupplies();

                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }





    }
}
