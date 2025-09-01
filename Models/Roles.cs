using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate_POS_Api.Models
{
    public class Roles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RoleId { get; set; }
        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; }
        [MaxLength(50)]
        public string Description { get; set; }
        [Required]
        [MaxLength(50)]

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        public bool IsSystemRole { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public ICollection<RolePermissions> Permissions { get; set; }
        public ICollection<UserRoles> UserRoles { get; set; }
    }
}
