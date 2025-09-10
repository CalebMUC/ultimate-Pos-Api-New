using Microsoft.AspNetCore.Authorization;

namespace Ultimate_POS_Api.Helper.Auth
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        //marker Interface for requirements in ASP.NET Core Authorization
        //each requirement Represents one permission
        public string Permission { get; set; }
        public PermissionRequirement(string permission)
        {
            this.Permission = permission;
        }
    }
}
