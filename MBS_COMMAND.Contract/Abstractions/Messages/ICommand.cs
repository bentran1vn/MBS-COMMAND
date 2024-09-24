using MassTransit;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MediatR;

namespace MBS_COMMAND.Contract.Abstractions.Messages;

[ExcludeFromTopology]
public interface ICommand : IRequest<Result>
{
}

[ExcludeFromTopology]
public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
