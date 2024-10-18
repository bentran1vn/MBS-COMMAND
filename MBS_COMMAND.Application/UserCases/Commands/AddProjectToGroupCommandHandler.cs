using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Projects;
using MBS_COMMAND.Domain.Abstractions;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Application.UserCases.Commands;

public class AddProjectToGroupCommandHandler(
    IRepositoryBase<Group, Guid> groupRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService)
    : ICommandHandler<Command.AddProject>
{
    public async Task<Result> Handle(Command.AddProject request, CancellationToken cancellationToken)
    {
        var group = await groupRepository.FindByIdAsync(request.GroupId, cancellationToken);
        if (group == null)
        {
            return Result.Failure(new Error("404", "Group not found"));
        }
        if (group.LeaderId.ToString() != currentUserService.UserId)
        {
            return Result.Failure(new Error("403", "Only group leader can add project to group"));
        }
        if(group.ProjectId!=null)
        {
            return Result.Failure(new Error("400", "Group already has a project"));
        }
       
        group.Project=new Project
        {
            Name=request.Name,
            Description=request.Description,
        };
        groupRepository.Update(group);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}