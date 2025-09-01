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
    public class NotificationController : ControllerBase
    {

        private readonly INotificationRepository _notificationRepository;
        public NotificationController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        [HttpPost("GetAccessToken")]
        [Authorize]
        public async Task<ActionResult> GetAccessToken()
        {
            try
            {
                var response = await _notificationRepository.GetAccessToken();
                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpPost("AddContact")]
        [Authorize]
        public async Task<ActionResult> AddContact(ContactListDto contacts)
        {
            try
            {
                var response = await _notificationRepository.AddContact(contacts);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet("GetContacts")]
        [Authorize]
        public async Task<ActionResult> GetContacts()
        {
            try
            {
                var response = await _notificationRepository.GetContacts();

                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost("AddNotification")]
        [Authorize]
        public async Task<ActionResult> AddNotification(NotificationListDTO notificationList)
        {
            try
            {
                var response = await _notificationRepository.AddNotification(notificationList);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        // [HttpPost("Sendsms")]
        // [Authorize]
        // public async Task<ActionResult> Sendsms(SmsRequestListDTO SmsPayload)
        // {
        //     try
        //     {
        //         var response = await _notificationRepository.Sendsms(SmsPayload);

        //         return Ok(response);
        //     }
        //     catch (Exception ex)
        //     {

        //         return BadRequest(ex.Message);
        //     }

        // }

        [HttpPost("AddSmsTemplate")]
        [Authorize]
        public async Task<ActionResult> AddSmsTemplate(MessageTemplateListDTO TemplatePayload)
        {
            try
            {

                var response = await _notificationRepository.AddSmsTemplate(TemplatePayload);

                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetSmsTemplates")]
        [Authorize]
        public async Task<ActionResult> GetSmsTemplates()
        {
            try
            {

                var response = await _notificationRepository.GetSmsTemplates();

                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }




    }
}
