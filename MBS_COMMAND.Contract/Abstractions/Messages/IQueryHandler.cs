using MBS_COMMAND.Contract.Abstractions.Shared;
using MediatR;

namespace MBS_COMMAND.Contract.Abstractions.Messages;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
