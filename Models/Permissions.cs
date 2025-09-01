using System.ComponentModel.DataAnnotations;

namespace Ultimate_POS_Api.Models
{
    public class Permissions
    {
        [Key]
        public Guid PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public string Module { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }

        public ICollection<RolePermissions> RolePermissions { get; set; }

    }
}
