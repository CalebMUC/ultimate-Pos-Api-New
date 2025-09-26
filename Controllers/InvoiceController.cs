using Microsoft.AspNetCore.Mvc;
using Ultimate_POS_Api.DTOS.Invoices;
using Ultimate_POS_Api.Models;
using Ultimate_POS_Api.Repository.Invoices;
using Ultimate_POS_Api.Repository.Purchases;

namespace Ultimate_POS_Api.Controllers
{
    public class InvoiceController : ControllerBase
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly iInvoiceRepo _invoiceRepo;
        public InvoiceController(Logger<InvoiceController> logger , iInvoiceRepo invoiceRepo)
        {
            _invoiceRepo = invoiceRepo;
            _logger = logger;
        }

        [HttpGet("GetAllPurchaseInvoice")]
        public async Task<IActionResult> GetAllPurchaseInvoice()
        {
            try
            {
                var response = await _invoiceRepo.GetPurchaseInvoicesAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all purchase invoices.");
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("AddPurchaseInvoice")]
        public async Task<IActionResult> AddPurchaseInvoice([FromBody] DTOS.Invoices.PurchasesInvoiceDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("No DATA");
                }
                var response = await _invoiceRepo.AddPurchaseInvoiceAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a purchase invoice.");
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("EditPurchaseInvoice")]
        public async Task<IActionResult> EditPurchaseInvoice([FromBody] DTOS.Invoices.EditPurchaseInvoiceDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("No DATA");
                }
                var response = await _invoiceRepo.EditPurchaseInvoiceAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while editing a purchase invoice.");
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeletePurchaseInvoice/{purchaseInvoiceId}")]
        public async Task<IActionResult> DeletePurchaseInvoice([FromRoute] Guid purchaseInvoiceId)
        {
            try
            {
                if (purchaseInvoiceId == Guid.Empty)
                {
                    return BadRequest("No DATA");
                }
                var response = await _invoiceRepo.DeletePurchaseInvoiceAsync(purchaseInvoiceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting a purchase invoice.");
                return BadRequest(ex.Message);
            }
        }


        // -------------------------------------  SalesInvoice ---------------------------------
        [HttpGet("GetAllSalesInvoice")]
        public async Task<IActionResult> GetAllSalesInvoice()
        {
            try
            {
                var response = await _invoiceRepo.GetSalesInvoicesAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all sales invoices.");
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("AddSalesInvoice")]
        public async Task<IActionResult> AddSalesInvoice([FromBody] DTOS.Invoices.SalesInvoiceDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("No DATA");
                }
                var response = await _invoiceRepo.AddSalesInvoiceAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a sales invoice.");
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("EditSalesInvoice")]
        public async Task<IActionResult> EditSalesInvoice([FromBody] DTOS.Invoices.EditSalesInvoiceDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("No DATA");
                }
                var response = await _invoiceRepo.EditSalesInvoiceAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while editing a sales invoice.");
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteSalesInvoice/{salesInvoiceId}")]
        public async Task<IActionResult> DeleteSalesInvoice([FromRoute] Guid salesInvoiceId)
        {
            try
            {
                if (salesInvoiceId == Guid.Empty)
                {
                    return BadRequest("No DATA");
                }
                var response = await _invoiceRepo.DeleteSalesInvoiceAsync(salesInvoiceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting a sales invoice.");
                return BadRequest(ex.Message);
            }
        }
    }
}
