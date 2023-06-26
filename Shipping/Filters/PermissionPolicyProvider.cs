using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;


namespace Shipping.Filters
{
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        public DefaultAuthorizationPolicyProvider FallbackPlicyProvider { get; }


        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options , IHttpContextAccessor httpContextAccessor)
        {
            FallbackPlicyProvider = new DefaultAuthorizationPolicyProvider(options);
            _httpContextAccessor = httpContextAccessor;

        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return FallbackPlicyProvider.GetDefaultPolicyAsync();
        }

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        {
            var isAuthenticated = _httpContextAccessor.HttpContext?.User.Identity.IsAuthenticated;
            if (isAuthenticated == true)
            {

                return FallbackPlicyProvider.GetDefaultPolicyAsync();
            }
            else { return Task.FromResult<AuthorizationPolicy>(null); }
        }

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith("Permissions", StringComparison.OrdinalIgnoreCase))
            {
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new PermissionRequirement(policyName));
                return Task.FromResult(policy.Build());
            
            }

            return FallbackPlicyProvider.GetPolicyAsync(policyName);
        }
    }
}
