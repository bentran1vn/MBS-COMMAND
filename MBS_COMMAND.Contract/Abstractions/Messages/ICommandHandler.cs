using MBS_COMMAND.Contract.Abstractions.Shared;
using MediatR;

namespace MBS_COMMAND.Contract.Abstractions.Messages;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}
