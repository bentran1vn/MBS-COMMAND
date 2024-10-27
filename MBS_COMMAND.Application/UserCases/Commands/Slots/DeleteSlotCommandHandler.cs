using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Slots;
using MBS_COMMAND.Domain.Abstractions;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;
using MBS_CONTRACT.SHARE.Services.Slots;

namespace MBS_COMMAND.Application.UserCases.Commands.Slots;
public class DeleteSlotCommandHandler : ICommandHandler<Command.DeleteSlot>
{
    private readonly IRepositoryBase<Slot, Guid> _repositoryBase;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSlotCommandHandler(IRepositoryBase<Slot, Guid> repositoryBase, IUnitOfWork unitOfWork)
    {
        _repositoryBase = repositoryBase;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(Command.DeleteSlot request, CancellationToken cancellationToken)
    {
        var slot = await _repositoryBase.FindByIdAsync(request.Id, cancellationToken);
        if (slot == null)
        {
            return Result.Failure(new Error("404", "Slot not found"));
        }
        slot.SlotUpdated(slot);
        _repositoryBase.Remove(slot);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}