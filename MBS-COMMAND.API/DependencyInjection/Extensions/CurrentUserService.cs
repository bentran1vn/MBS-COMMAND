using MBS_COMMAND.Domain.Abstractions.Repositories;
using System.Security.Claims;

namespace MBS_COMMAND.API.DependencyInjection.Extensions;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private readonly ClaimsPrincipal? _claimsPrincipal = httpContextAccessor?.HttpContext?.User;

    public string? UserId => _claimsPrincipal?.FindFirstValue("UserId");
    public string? Role => _claimsPrincipal?.FindFirstValue(ClaimTypes.Role);

    public string? UserName => _claimsPrincipal?.FindFirstValue(ClaimTypes.Name);

}
