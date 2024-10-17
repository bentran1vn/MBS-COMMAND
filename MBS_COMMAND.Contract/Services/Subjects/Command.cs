using MBS_COMMAND.Contract.Abstractions.Messages;

namespace MBS_COMMAND.Contract.Services.Subjects;

public static class Command
{
    public record AddSubject(string Name) : ICommand;
}