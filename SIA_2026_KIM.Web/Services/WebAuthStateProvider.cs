using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;

namespace SIA_2026_KIM.Web.Services
{
    public class WebAuthStateProvider : RevalidatingServerAuthenticationStateProvider
    {
        public WebAuthStateProvider(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        // How often the circuit re-checks that the user is still authenticated
        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

        protected override Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            // The state stays valid only while the cookie identity is authenticated
            var isValid = authenticationState.User.Identity?.IsAuthenticated == true;
            return Task.FromResult(isValid);
        }
    }
}
