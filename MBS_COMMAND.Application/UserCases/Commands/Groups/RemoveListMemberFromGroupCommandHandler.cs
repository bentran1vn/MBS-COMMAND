using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Groups;
using MBS_COMMAND.Domain.Abstractions;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Application.UserCases.Commands.Groups;

public sealed class RemoveListMemberFromGroupCommandHandler(IRepositoryBase<Group, Guid> groupRepository, IRepositoryBase<User, Guid> userRepository, IUnitOfWork unitOfWork) : ICommandHandler<Command.RemoveListMemberFromGroup>
{
    public async Task<Result> Handle(Command.RemoveListMemberFromGroup request, CancellationToken cancellationToken)
    {
        Dictionary<Guid, string> MembersNotFound = [];
        var G = await groupRepository.FindSingleAsync(x => x.Id == request.GroupId, cancellationToken);
        if (G == null)
            return Result.Failure(new Error("404", "Group Not Found"));
        if (request.MemberId.Contains((Guid)G.LeaderId))
            return Result.Failure(new Error("422", "Mentor cannot be removed from group"));
        foreach (var memberId in request.MemberId)
            {
            var user = await userRepository.FindByIdAsync(memberId);
            if (user == null)
            {
                MembersNotFound.Add(memberId, "Member Not Found");
            }
            else
            {
                var member = G.Members!.FirstOrDefault(x => x.StudentId == user!.Id);
                if (member == null)
                {
                    MembersNotFound.Add(memberId, "Member Not Found in Group");
                }
                else
                {
                    G.Members!.Remove(member);
                    groupRepository.Update(G);
                    try
                    {
                        await unitOfWork.SaveChangesAsync(cancellationToken);
                    }
                    catch
                    {
                        MembersNotFound.Add(memberId, "Member already removed from group");
                    }
                }
            }

        }
        var returns = string.Join(", ", MembersNotFound.Select(x => $"MemberId: {x.Key}, Error: {x.Value}"));
        return MembersNotFound.Count > 0 ? Result.Failure(new Error("422", returns)) : Result.Success();

    }
}
