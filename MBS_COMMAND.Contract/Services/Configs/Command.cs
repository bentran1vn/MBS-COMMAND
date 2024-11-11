using MBS_COMMAND.Contract.Abstractions.Messages;

namespace MBS_COMMAND.Contract.Services.Configs;
public static class Command
{
    public record GeneratePoints() : ICommand;
    public record GeneratePointsForAllGroup() : ICommand;
}