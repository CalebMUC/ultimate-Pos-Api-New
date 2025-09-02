namespace Ultimate_POS_Api.DTOS.Permissions
{
    public class GetPermissionDto
    {
        public Guid PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public string Module { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }

    }
}
