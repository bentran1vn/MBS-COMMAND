using MBS_AUTHORIZATION.Domain.Entities;
using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Groups;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Application.UserCases.Commands.Groups;

public sealed class CreateGroupCommandHandler : ICommandHandler<Command.CreateGroupCommand>
{
    private readonly IRepositoryBase<Group, Guid> _groupRepository;
    private readonly IRepositoryBase<User, Guid> _userRepository;

    public CreateGroupCommandHandler(IRepositoryBase<Group, Guid> repositoryBase, IRepositoryBase<User, Guid> userRepository)
    {
        _groupRepository = repositoryBase;
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(Command.CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var M = await _userRepository.FindSingleAsync(x => x.Id == request.MentorId, cancellationToken);
        if (M == null)
            return Result.Failure(new Error("404", "Mentor Not Found"));
        var G = new Group
        {
            Name = request.Name,
            MentorId = request.MentorId,
            Stack = request.Stacks,

        };
        _groupRepository.Add(G);
        return Result.Success();
    }
}
