using System.ComponentModel.DataAnnotations;

namespace Ultimate_POS_Api.Models
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ActionType { get; set; } 
        public string EntityName { get; set; }
        public DateTime Timestamp { get; set; }
        public string IpAddress { get; set; }
        public string Device { get; set; }
    }
}
