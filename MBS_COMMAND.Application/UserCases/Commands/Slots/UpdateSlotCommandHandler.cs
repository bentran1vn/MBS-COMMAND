using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Slots;
using MBS_COMMAND.Domain.Abstractions;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Application.UserCases.Commands.Slots;
public class UpdateSlotCommandHandler(
    IUnitOfWork unitOfWork,
    IRepositoryBase<Slot, Guid> slotRepository)
    : ICommandHandler<Command.UpdateSlot>
{
    public async Task<Result> Handle(Command.UpdateSlot request, CancellationToken cancellationToken)
    {
        var slot = await slotRepository.FindByIdAsync((Guid)request.SlotModel.Id!, cancellationToken);
        if (slot == null)
        {
            return Result.Failure(new Error("404", "Slot not found"));
        }

        slot.StartTime = TimeOnly.Parse(request.SlotModel.StartTime);
        slot.EndTime = TimeOnly.Parse(request.SlotModel.EndTime);
        slot.Date = DateOnly.Parse(request.SlotModel.Date);
        slot.IsOnline = request.SlotModel.IsOnline;
        slot.Note = request.SlotModel.Note;
        slotRepository.Update(slot);
        slot.SlotUpdated(slot);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}