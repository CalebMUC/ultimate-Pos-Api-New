using System.ComponentModel.DataAnnotations;

namespace Ultimate_POS_Api.Models
{
    public class UserRoles
    {
        [Key]
        public int UserRoleId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public DateTime AssignedAt { get; set; }
        public string AssignedBy { get; set; }

        public User User { get; set; }
        public Roles Role { get; set; }
    }
}
