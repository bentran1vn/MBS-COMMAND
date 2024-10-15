using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Slots;
using MBS_COMMAND.Domain.Abstractions;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;
using static System.DateOnly;
namespace MBS_COMMAND.Application.UserCases.Commands.Slots;
                
public sealed class CreateSlotCommandHandler(
    IRepositoryBase<Slot, Guid> slotRepository,
    IRepositoryBase<User, Guid> userRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<Command.CreateSlot>
{
    public async Task<Result> Handle(Command.CreateSlot request, CancellationToken cancellationToken)
    {
        var mentor = await userRepository.FindByIdAsync(request.MentorId, cancellationToken);
        if (mentor == null)
            return Result.Failure(new Error("404", "User Not Found"));

        var newSlots = request.SlotModels.Select(slotModel => new Slot
        {
            Id = Guid.NewGuid(),
            MentorId = mentor.Id,
            StartTime = TimeOnly.Parse(slotModel.StartTime),
            EndTime = TimeOnly.Parse(slotModel.EndTime),
            Date = Parse(slotModel.Date),
            Note = slotModel.Note,
            IsOnline = slotModel.IsOnline,
            Month = (short?)Parse(slotModel.Date).Month
        }).ToList();

        // Check for overlaps within the request
        var overlapResult = CheckForOverlapsInRequest(newSlots);
        if (overlapResult.IsFailure)
            return overlapResult;

        // Fetch existing slots for the mentor
        var existingSlots = slotRepository.FindAll(x => x.MentorId == mentor.Id);

        // Check for overlaps with existing slots
        foreach (var newSlot in newSlots)
        {
            if (HasOverlap(newSlot, existingSlots))
            {
                return Result.Failure(new Error("409", $"Slot overlaps with existing slot: {newSlot.Date} {newSlot.StartTime}-{newSlot.EndTime}"));
            }
        }

        slotRepository.AddRange(newSlots);
        mentor.CreateSlot(newSlots, mentor.Id);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }

    private static Result CheckForOverlapsInRequest(List<Slot> slots)
    {
        for (var i = 0; i < slots.Count; i++)
        {
            for (var j = i + 1; j < slots.Count; j++)
            {
                if (AreOverlapping(slots[i], slots[j]))
                {
                    return Result.Failure(new Error("409", $"Overlapping slots in request: " +
                        $"{slots[i].Date} {slots[i].StartTime}-{slots[i].EndTime} and " +
                        $"{slots[j].Date} {slots[j].StartTime}-{slots[j].EndTime}"));
                }
            }
        }
        return Result.Success();
    }

    private static bool HasOverlap(Slot newSlot, IEnumerable<Slot> existingSlots)
    {
        return existingSlots.Any(existingSlot => AreOverlapping(newSlot, existingSlot));
    }

    private static bool AreOverlapping(Slot slot1, Slot slot2)
    {
        return slot1.Date == slot2.Date &&
               slot1.StartTime < slot2.EndTime &&
               slot2.StartTime < slot1.EndTime;
    }
}
