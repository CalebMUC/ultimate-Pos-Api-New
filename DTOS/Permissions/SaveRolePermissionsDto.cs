namespace Ultimate_POS_Api.DTOS.Permissions
{
    public class SaveRolePermissionsDto
    {
        public Guid RoleId { get; set; }
        public List<Guid> Permissions { get; set; } = new List<Guid>();
    }
}
