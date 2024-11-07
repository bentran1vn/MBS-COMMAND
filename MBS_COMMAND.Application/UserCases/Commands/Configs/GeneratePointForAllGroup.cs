using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Configs;
using MBS_COMMAND.Domain.Abstractions;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;
using MBS_COMMAND.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MBS_COMMAND.Application.UserCases.Commands.Configs;
public class GeneratePointForAllGroup : ICommandHandler<Command.GeneratePointsForAllGroup>
{
    private readonly IRepositoryBase<Group, Guid> _groupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GeneratePointForAllGroup(IRepositoryBase<Group, Guid> groupRepository,
        IUnitOfWork unitOfWork)
    {
        _groupRepository = groupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(Command.GeneratePointsForAllGroup request, CancellationToken cancellationToken)
    {
        var groups = await _groupRepository.FindAll().AsTracking().ToListAsync(cancellationToken);
        foreach (var x in groups)
        {
            var totalPoints = x.Members.Sum(x => x.Student.Points);
            x.BookingPoints = totalPoints;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}