using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Mentors;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;
using MBS_COMMAND.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MBS_COMMAND.Application.UserCases.Commands.Groups;
public class
    MentorAcceptOrDeclineFromGroupCommandHandler : ICommandHandler<Command.MentorAcceptOrDeclineFromGroupCommand>
{
    private readonly ApplicationDbContext _context;
    private readonly IRepositoryBase<Group, Guid> _groupRepository;
    private readonly IRepositoryBase<User, Guid> _userRepository;

    public MentorAcceptOrDeclineFromGroupCommandHandler(ApplicationDbContext context,
        IRepositoryBase<Group, Guid> groupRepository, IRepositoryBase<User, Guid> userRepository)
    {
        _context = context;
        _groupRepository = groupRepository;
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(Command.MentorAcceptOrDeclineFromGroupCommand request,
        CancellationToken cancellationToken)
    {
        var mentor = await _userRepository.FindByIdAsync(request.MentorId, cancellationToken);
        var group = await _groupRepository.FindByIdAsync(request.GroupId, cancellationToken);
        var IsMentorAcceptedOrDeclined =
            await _context.Configs.FirstOrDefaultAsync(x => x.Key.Equals($"{group.Name}MentorInvite"),
                cancellationToken);
        if (IsMentorAcceptedOrDeclined == null)
        {
            return Result.Failure(new Error("400", "Group not found or mentor not invited"));
        }

        switch (IsMentorAcceptedOrDeclined.Value)
        {
            case "Accepted":
                return Result.Failure(new Error("400", "Mentor already accepted"));
            case "Declined":
                return Result.Failure(new Error("400", "Mentor already declined"));
        }

        if (request.IsAccepted)
        {
            IsMentorAcceptedOrDeclined.Value = "Accepted";
            group.MentorId = request.MentorId;
            _groupRepository.Update(group);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();

        }

        IsMentorAcceptedOrDeclined.Value = "Declined";
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

}