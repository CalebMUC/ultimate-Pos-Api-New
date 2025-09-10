using Microsoft.AspNetCore.Authorization;

namespace Ultimate_POS_Api.Helper.Auth
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            // Check if user has the required permission claim
            var hasPermission = context.User.FindAll("permissions")
                .Any(c => string.Equals(c.Value, requirement.Permission, StringComparison.OrdinalIgnoreCase));

            if (hasPermission)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
