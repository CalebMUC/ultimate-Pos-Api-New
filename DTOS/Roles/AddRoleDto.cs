using System.ComponentModel.DataAnnotations;

namespace Ultimate_POS_Api.DTOS.Roles
{
    public class AddRoleDto
    {
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
    }
}
