using Microsoft.AspNetCore.Mvc;
using Ultimate_POS_Api.DTOS.Till;
using Ultimate_POS_Api.Repository.TillRepo;

namespace Ultimate_POS_Api.Controllers
{
    public class TillManagementController : ControllerBase
    {
        private readonly ITillRepository _tillRepository;
        private ILogger<TillManagementController> _logger;

        public TillManagementController(ITillRepository tillRepository,
            ILogger<TillManagementController> logger) {
            _tillRepository = tillRepository;
            _logger = logger;

        }
        [HttpPost("AddTill")]
        public async Task<ActionResult> AddTill([FromBody]AddTillDto addTill)
        {
            try
            {
                var response = await _tillRepository.AddTill(addTill);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding till");
                return BadRequest(ex.Message);
            }
        }
        public async Task<ActionResult> UpdateTill()
        {
            return Ok();
        }
        public async Task<ActionResult> DeleteTill()
        {
            return Ok();
        }
        public async Task<ActionResult> GetTill()
        {
            return Ok();
        }
        public async Task<ActionResult> AssignTill()
        {
            return Ok();
        }
        public async Task<ActionResult> OpenTill()
        {
            return Ok();
        }
        public async Task<ActionResult> CloseTill()
        {
            return Ok();
        }

    }
}
