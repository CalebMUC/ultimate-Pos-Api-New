using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Ultimate_POS_Api.DTOS
{
    public class NotificationDTO
    {
        public string MessageID { get; set; }
        public string PhoneNumber { get; set; }
        public string TextHeader { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        // public string Date { get; set; } 
        public string Module { get; set; }

        public List<SendSmsRequestDTO> SmsRequest { get; set; }
    }

    public class SendSmsRequestDTO
    {
        public string Username { get; set; }
        public string To { get; set; }
        public string Header { get; set; }
        public string SenderId { get; set; }
        public string Message { get; set; }
        // public string Token { get; set; }
    }

    public class MessageTemplateDTO
    {
        public string TemplateID { get; set; }
        public string TemplateHeader { get; set; }
        public string TemplateBody { get; set; }
    }

    public class ContactDTO
    {
        public string ClientID { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }



    public class MessageTemplateListDTO
    {
        [Required]
        public IList<MessageTemplateDTO> Template { get; set; } = new List<MessageTemplateDTO>();
    }

    public class NotificationListDTO
    {
        [Required]
        public IList<NotificationDTO> Notification { get; set; } = new List<NotificationDTO>();
    }

    public class ContactListDto
    {
        [Required]
        public IList<ContactDTO> Contact { get; set; } = new List<ContactDTO>();
    }

    public class SmsRequestListDTO
    {
        [Required]
        public IList<SendSmsRequestDTO> SmsPayload { get; set; } = new List<SendSmsRequestDTO>();
    }

}

