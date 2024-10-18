using MBS_COMMAND.Contract.Abstractions.Messages;

namespace MBS_COMMAND.Contract.Services.Projects;

public static class Command
{
    public record AddProject(string Name, string Description, Guid GroupId) : ICommand;
}