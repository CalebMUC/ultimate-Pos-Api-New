namespace Ultimate_POS_Api.DTOS.Roles
{
    public class GetRolesDto
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsSystemRole { get; set; }
        public bool IsActive { get; set; }
    }
}
