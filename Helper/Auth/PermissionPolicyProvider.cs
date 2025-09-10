using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Ultimate_POS_Api.Helper.Auth
{
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        //Create Permission Prefix
        public const string POLICY_PREFIX = "PERMISSION:";
        private readonly DefaultAuthorizationPolicyProvider _fallback;

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options) {
            _fallback = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase))
            {
                var permission = policyName.Substring(POLICY_PREFIX.Length);
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new PermissionRequirement(permission));
                return Task.FromResult<AuthorizationPolicy?>(policy.Build());
            }
            return _fallback.GetPolicyAsync(policyName);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _fallback.GetDefaultPolicyAsync();
        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => _fallback.GetFallbackPolicyAsync();
    }
}
