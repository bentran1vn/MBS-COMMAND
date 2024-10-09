using MBS_COMMAND.Domain.Abstractions.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MBS_COMMAND.API.DependencyInjection.Extensions;

public class CurrentUserService : ICurrentUserService
{
    private readonly ClaimsPrincipal? _claimsPrincipal;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _claimsPrincipal = httpContextAccessor?.HttpContext?.User;
       
    }

    public string? UserId => _claimsPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? UserName => _claimsPrincipal?.FindFirstValue(ClaimTypes.Name);

}
