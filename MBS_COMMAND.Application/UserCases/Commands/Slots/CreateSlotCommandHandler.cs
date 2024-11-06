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
    ICurrentUserService currentUserService,
    IRepositoryBase<Semester, Guid> semesterRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<Command.CreateSlot>
{
    public async Task<Result> Handle(Command.CreateSlot request, CancellationToken cancellationToken)
    {
        // Parallel fetch of mentor and current semester
        var mentor =
            await userRepository.FindSingleAsync(x => x.Id == Guid.Parse(currentUserService.UserId!) && x.Status == 1,
                cancellationToken);
        if (mentor == null)
            return Result.Failure(new Error("404", "User Not Found"));

        var currentSemester = await semesterRepository.FindSingleAsync(x => x.IsActive, cancellationToken);
        if (currentSemester == null)
            return Result.Failure(new Error("404", "Active semester not found"));

        var semesterEndDate = currentSemester.From.AddDays(6);

        // Validate all dates in parallel (no DB operations here, so parallel is safe)
        var invalidSlots = request.SlotModels
            .AsParallel()
            .Select(x =>
            {
                var date = Parse(x.Date);
                var startTime = TimeOnly.Parse(x.StartTime);
                var endTime = TimeOnly.Parse(x.EndTime);
                var duration = endTime - startTime;

                return new
                {
                    x.Date,
                    StartTime = startTime,
                    EndTime = endTime,
                    IsDateInvalid = date < currentSemester.From || date > semesterEndDate,
                    IsDurationInvalid = duration < TimeSpan.FromMinutes(30) || duration > TimeSpan.FromHours(1)
                };
            })
            .Where(x => x.IsDateInvalid || x.IsDurationInvalid)
            .ToList();

        if (invalidSlots.Count != 0)
        {
            var dateErrors = invalidSlots
                .Where(x => x.IsDateInvalid)
                .Select(x => x.Date);

            var durationErrors = invalidSlots
                .Where(x => x.IsDurationInvalid)
                .Select(x => $"{x.Date} {x.StartTime}-{x.EndTime}");

            var errorMessages = new List<string>();

            if (dateErrors.Any())
                errorMessages.Add(
                    $"Slot dates {string.Join(", ", dateErrors)} must be within the first week of the current semester");

            if (durationErrors.Any())
                errorMessages.Add(
                    $"Slots must be between 30 minutes and 1 hour long. Invalid slots: {string.Join(", ", durationErrors)}");

            return Result.Failure(new Error("400", string.Join(". ", errorMessages)));
        }

        // Create slots (parallel is safe here as it's just object creation)
        var newSlots = request.SlotModels
            .AsParallel()
            .Select(slotModel => new Slot
            {
                Id = Guid.NewGuid(),
                MentorId = mentor.Id,
                StartTime = TimeOnly.Parse(slotModel.StartTime),
                EndTime = TimeOnly.Parse(slotModel.EndTime),
                Date = Parse(slotModel.Date),
                Note = slotModel.Note,
                IsOnline = slotModel.IsOnline,
                Month = (short)Parse(slotModel.Date).Month
            })
            .ToList();

        // Check for overlaps within the request (no DB operations)
        var overlapResult = CheckForOverlapsInRequestOptimized(newSlots);
        if (overlapResult.IsFailure)
            return overlapResult;

        // Fetch existing slots synchronously to avoid context threading issues
        var existingSlots = slotRepository
            .FindAll(x => x.MentorId == mentor.Id)
            .ToList();

        // Check for overlaps (no DB operations)
        var overlappingSlots = FindOverlappingSlots(newSlots, existingSlots);

        if (overlappingSlots.Count != 0)
        {
            var firstOverlap = overlappingSlots.First();
            return Result.Failure(new Error("409",
                $"Slot overlaps with existing slot: {firstOverlap.Date} {firstOverlap.StartTime}-{firstOverlap.EndTime}"));
        }

        // Use regular AddRange since AddRangeAsync isn't available
        slotRepository.AddRange(newSlots);
        mentor.CreateSlot(newSlots);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static Result CheckForOverlapsInRequestOptimized(List<Slot> slots)
    {
        var sortedSlots = slots
            .OrderBy(s => s.Date)
            .ThenBy(s => s.StartTime)
            .ToList();

        for (var i = 0; i < sortedSlots.Count - 1; i++)
        {
            var current = sortedSlots[i];
            var next = sortedSlots[i + 1];

            if (current.Date == next.Date && current.EndTime > next.StartTime)
            {
                return Result.Failure(new Error("409",
                    $"Overlapping slots in request: {current.Date} {current.StartTime}-{current.EndTime} and " +
                    $"{next.Date} {next.StartTime}-{next.EndTime}"));
            }
        }

        return Result.Success();
    }

    private static List<Slot> FindOverlappingSlots(List<Slot> newSlots, List<Slot> existingSlots)
    {
        var existingSlotsByDate = existingSlots
            .GroupBy(s => s.Date)
            .ToDictionary(g => g.Key, g => g.ToList());

        return newSlots
            .Where(newSlot =>
                existingSlotsByDate.TryGetValue(newSlot.Date, out var slotsOnSameDay) &&
                slotsOnSameDay.Any(existingSlot =>
                    newSlot.StartTime < existingSlot.EndTime &&
                    existingSlot.StartTime < newSlot.EndTime))
            .ToList();
    }
}