using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Groups;
using MBS_COMMAND.Domain.Abstractions;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Application.UserCases.Commands.Groups;

public sealed class AddListMemberToGroupCommandHandler(IRepositoryBase<User, Guid> userRepository, IRepositoryBase<Group, Guid> groupRepository, IUnitOfWork unitOfWork) : ICommandHandler<Command.AddListMemberToGroup>
{
    public async Task<Result> Handle(Command.AddListMemberToGroup request, CancellationToken cancellationToken)
    {
        //validate all MemberId
        Dictionary<Guid, string> MembersNotFound = [];
        var G = await groupRepository.FindSingleAsync(x => x.Id == request.GroupId, cancellationToken);
        if (G == null)
            return Result.Failure(new Error("404", "Group Not Found"));
        foreach (var memberId in request.MemberId)
        {
            var user = await userRepository.FindByIdAsync(memberId);
            if (user == null)
            {
                MembersNotFound.Add(memberId, "Member Not Found");
            }
            else
            {
                G.Members!.Add(new Group_Student_Mapping { StudentId = user!.Id, GroupId = G.Id });
                groupRepository.Update(G);
                try
                {
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                }
                catch 
                {
                    MembersNotFound.Add(memberId, "Member already joined group");
                }
            }

        }
        var returns = string.Join(", ", MembersNotFound.Select(x => $"MemberId: {x.Key}, Error: {x.Value}"));


        return MembersNotFound.Count > 0 ? Result.Failure(new Error("422", returns)) : Result.Success();
    }
}
