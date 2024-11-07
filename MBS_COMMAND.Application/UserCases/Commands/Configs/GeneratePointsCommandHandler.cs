using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Configs;
using MBS_COMMAND.Domain.Abstractions;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;
using MBS_COMMAND.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MBS_COMMAND.Application.UserCases.Commands.Configs;
public class GeneratePointsCommandHandler : ICommandHandler<Command.GeneratePoints>
{
    private readonly IRepositoryBase<User, Guid> _userRepository;
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public GeneratePointsCommandHandler(IRepositoryBase<User, Guid> userRepository, ApplicationDbContext context,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(Command.GeneratePoints request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.FindAll(x => x.Role == 0 && x.Status == 1).AsTracking()
            .ToListAsync(cancellationToken);
        var point = await _context.Configs.Where(x => x.Key.Equals("PointPerStudent")).Select(x => x.Value)
            .FirstOrDefaultAsync(cancellationToken);
        foreach (var x in users)
        {
            x.Points = point;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}