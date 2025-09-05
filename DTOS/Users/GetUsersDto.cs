using System.ComponentModel.DataAnnotations;

namespace Ultimate_POS_Api.DTOS.Users
{
    public class GetUsersDto
    {
        public Guid UserId { get; set; } = Guid.NewGuid();

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }

        // ✅ Role management - pick one approach
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
