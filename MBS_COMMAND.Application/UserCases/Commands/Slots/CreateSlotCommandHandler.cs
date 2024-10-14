using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Slots;
using MBS_COMMAND.Domain.Abstractions;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Application.UserCases.Commands.Slots;

public class CreateSlotCommandHandler : ICommandHandler<Command.CreateSlot>
{
    private readonly IRepositoryBase<Slot, Guid> _slotRepository;
    private readonly IRepositoryBase<User, Guid> _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSlotCommandHandler(IRepositoryBase<Slot, Guid> slotRepository, IRepositoryBase<User, Guid> userRepository, IUnitOfWork unitOfWork)
    {
        _slotRepository = slotRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(Command.CreateSlot request, CancellationToken cancellationToken)
    {
        var mentor = await _userRepository.FindByIdAsync(request.MentorId);
        if (mentor == null)
            return Result.Failure(new Error("404", "User Not Found"));

        var newSlots = request.SlotModels.Select(slotModel => new Slot
        {
            Id = Guid.NewGuid(),
            MentorId = mentor.Id,
            StartTime = TimeOnly.Parse(slotModel.StartTime),
            EndTime = TimeOnly.Parse(slotModel.EndTime),
            Date = DateOnly.Parse(slotModel.Date),
            Note = slotModel.Note,
            IsOnline = slotModel.IsOnline,
            Month = (short?)DateOnly.Parse(slotModel.Date).Month
        }).ToList();

        // Check for overlaps within the request
        var overlapResult = CheckForOverlapsInRequest(newSlots);
        if (overlapResult.IsFailure)
            return overlapResult;

        // Fetch existing slots for the mentor
        var existingSlots = _slotRepository.FindAll(x => x.MentorId == mentor.Id);

        // Check for overlaps with existing slots
        foreach (var newSlot in newSlots)
        {
            if (HasOverlap(newSlot, existingSlots))
            {
                return Result.Failure(new Error("409", $"Slot overlaps with existing slot: {newSlot.Date} {newSlot.StartTime}-{newSlot.EndTime}"));
            }
        }

        _slotRepository.AddRange(newSlots);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    private Result CheckForOverlapsInRequest(List<Slot> slots)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            for (int j = i + 1; j < slots.Count; j++)
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

    private bool HasOverlap(Slot newSlot, IEnumerable<Slot> existingSlots)
    {
        return existingSlots.Any(existingSlot => AreOverlapping(newSlot, existingSlot));
    }

    private bool AreOverlapping(Slot slot1, Slot slot2)
    {
        return slot1.Date == slot2.Date &&
               slot1.StartTime < slot2.EndTime &&
               slot2.StartTime < slot1.EndTime;
    }
}
