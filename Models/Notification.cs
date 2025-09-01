using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate_POS_Api.Models
{
    public class Notification
    {
        [Key]
        [Column(TypeName = "VARCHAR(255)")]
        public string MessageID { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(20)")]
        public string PhoneNumber { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string TextHeader { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string Message { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string Status { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string Date { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string Module { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string CreatedBy { get; set; }

        [Required]
        [Column(TypeName = "JSON")]
        public string SmsRequest { get; set; }

    }

    public class Contacts
    {
        [Key]
        [Column(TypeName = "VARCHAR(255)")]
        public string ClientID { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string Username { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(12)")]
        public string PhoneNumber { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedOn { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "VARCHAR(255)")]
        public string UpdatedBy { get; set; }
    }

    public class MessageTemplates
    {
        [Key]
        [Column(TypeName = "VARCHAR(255)")]
        public string TemplateID { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string TemplateHeader { get; set; }


        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string TemplateBody { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string CreatedOn { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string CreatedBy { get; set; }
    }
}
