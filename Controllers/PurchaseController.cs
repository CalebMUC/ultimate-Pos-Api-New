using Microsoft.AspNetCore.Mvc;
using Ultimate_POS_Api.Repository.Purchases;

namespace Ultimate_POS_Api.Controllers
{
    public class PurchaseController : ControllerBase
    {
        private readonly ILogger<PurchaseController> _logger;
        private readonly IPurchaseRepo _purchaseRepo;
        public PurchaseController(ILogger<PurchaseController> logger,IPurchaseRepo repo) { 
            _logger = logger;
            _purchaseRepo = repo;
        }

        [HttpGet("GetAllPurchase")]
        public async Task<IActionResult> GetAllPurchase()
        {
            try
            {
                var response = await _purchaseRepo.GetAllPurchaseOrdersAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all purchase orders.");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddPurchaseOrder")]
        public async Task<IActionResult> AddPurchaseOrder([FromBody] DTOS.Purchases.AddPurchaseOrderDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("No DATA");
                }
                var response = await _purchaseRepo.AddPurchaseOrderAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a purchase order.");
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("EditPurchaseOrder")]
        public async Task<IActionResult> EditPurchaseOrder([FromBody] EditPurchaseOrder dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("No DATA");
                }
                var response = await _purchaseRepo.EditPurchaseOrderAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a purchase order.");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeletePurchaseOrder")]
        public async Task<IActionResult> DeletePurchaseOrder([FromQuery] Guid purchaseOrderId)
        {
            try
            {
                if (purchaseOrderId == Guid.Empty)
                {
                    return BadRequest("Invalid Purchase Order ID");
                }
                var response = await _purchaseRepo.DeletePurchaseOrderAsync(purchaseOrderId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting a purchase order.");
                return BadRequest(ex.Message);
            }
        }



    }
}
