namespace BackEndAje.Api.Infrastructure.Services.Security
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Logging;

    public class CustomClaimsTransformation : IClaimsTransformation
    {
        private readonly ILogger<CustomClaimsTransformation> _logger;

        public CustomClaimsTransformation(ILogger<CustomClaimsTransformation> logger)
        {
            this._logger = logger;
        }

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity?.IsAuthenticated == true)
            {
                var identity = (ClaimsIdentity)principal.Identity;

                // Buscar específicamente el claim "nameidentifier"
                var nameIdentifierClaim = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(nameIdentifierClaim))
                {
                    this._logger.LogInformation($"Transforming claim 'nameidentifier' to 'UserId': {nameIdentifierClaim}");
                    identity.AddClaim(new Claim("UserId", nameIdentifierClaim));
                }
                else
                {
                    this._logger.LogWarning("Claim 'nameidentifier' not found or empty.");
                }
            }

            return Task.FromResult(principal);
        }
    }
}
