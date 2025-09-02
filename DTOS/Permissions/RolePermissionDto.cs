namespace Ultimate_POS_Api.DTOS.Permissions
{
    public class RolePermissionDto
    {
        public Guid  RoleId { get; set; }
        public List<string> Permissions { get; set; }
    }
}
