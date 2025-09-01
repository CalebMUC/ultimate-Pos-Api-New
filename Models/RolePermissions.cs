using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate_POS_Api.Models
{
    public class RolePermissions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RolePermissionId { get; set; }
        [Required]
        public Guid RoleId { get; set; }
        [Required]
        public Guid PermissionId { get; set; }
        [Required]
        public DateTime GrantedAt { get; set; }
        [Required]
        public string GrantedBy { get; set; }

        // Navigation properties
        public Roles Role { get; set; }
        public Permissions Permission { get; set; }
    }
}
