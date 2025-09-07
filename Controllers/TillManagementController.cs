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
        [HttpPost("Add-Till")]
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
        [HttpPost("Update-Till")]
        public async Task<ActionResult> UpdateTill()
        {
            try
            {
                var response = await _tillRepository.UpdateTillAsync(new UpdateTillDto());
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        [HttpDelete("DeleteTill/{TillId}")]
        public async Task<ActionResult> DeleteTill(int TillId)
        {
            try
            {
                var response = await _tillRepository.DeleteTillAsync(TillId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        [HttpPost("Get-Till")]
        public async Task<ActionResult> GetTill()
        {
            try
            {
                var response = await _tillRepository.GetTillAsync();
                return Ok(response);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpPost("Assign-Till")]
        public async Task<ActionResult> AssignTill([FromBody]AssignTillDto assignTillDto)
        {
            try
            {
                var response = await _tillRepository.AssignTillAsync(assignTillDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Open-Till")]
        public async Task<ActionResult> OpenTill([FromBody]OpenTillDto openTillDto)
        {
            try
            {
                var response = await _tillRepository.OpenTillAsync(openTillDto);
                return Ok(response);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("submit-closure")]
        public async Task<IActionResult> SubmitClosure([FromBody] TillClosureDto dto)
        {
            try
            {
                var response = await _tillRepository.SubmitTillClosureAsync(dto);
                return Ok(response);
       }
            catch (Exception ex) {
                return BadRequest(ex.Message);

            }
        }
        [HttpPost("supervise-till")]
        public async Task<IActionResult> SuperviseTill([FromBody] ApproveTillDto dto)
        {
            try
            {
                var response = await _tillRepository.SuperviseTillAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Get-Tills-Under-Review")]
        public async Task<ActionResult> GetTillsUnderReview()
        {
            try
            {
                var response = await _tillRepository.GetTillsUnderReviewAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Close-Till")]
        public async Task<ActionResult> CloseTill(CloseTillDto closeTillDto)
        {
            try {
                var response = await _tillRepository.CloseTillAsync(closeTillDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

    }
}
