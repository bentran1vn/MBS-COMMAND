namespace MBS_COMMAND.Domain.Abstractions.Repositories;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? UserName { get; }
    string? Role { get; }
}
