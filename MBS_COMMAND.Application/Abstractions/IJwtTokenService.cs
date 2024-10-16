using System.Security.Claims;

namespace MBS_COMMAND.Application.Abstractions;

public interface IJwtTokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    (ClaimsPrincipal, bool) GetPrincipalFromExpiredToken(string token);
}