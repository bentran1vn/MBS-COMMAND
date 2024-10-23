using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Domain.Abstractions;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static MBS_COMMAND.Contract.Abstractions.Shared.Result;
using Command = MBS_COMMAND.Contract.Services.Slots.Command;

namespace MBS_COMMAND.Application.UserCases.Commands.Slots;
public sealed class GenerateSlotForSemesterCommandHandler(
    IRepositoryBase<Slot, Guid> slotRepository,
    IRepositoryBase<Semester, Guid> semesterRepository,
    IRepositoryBase<User, Guid> userRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<Command.GenerateSlotForSemester>
{
    public async Task<Result> Handle(Command.GenerateSlotForSemester request, CancellationToken cancellationToken)
    {
        // Get current semester
        var currentSemester = await semesterRepository.FindSingleAsync(x => x.IsActive, cancellationToken);
        if (currentSemester == null)
            return Failure(new Error("404", "No active semester found"));

        // Get all mentors in parallel with first week slots
        var mentors = await userRepository
            .FindAll(x => x.Role == 2)
            .AsTracking()
            .ToListAsync(cancellationToken);

        if (mentors.Count == 0)
            return Success(); // No mentors to process

        // Get first week slots for all mentors in one query
        var firstWeekEnd = currentSemester.From.AddDays(6);
        var allFirstWeekSlots = await slotRepository
            .FindAll(x =>
                mentors.Select(m => m.Id.ToString()).Contains(x.MentorId.ToString()) &&
                x.Date >= currentSemester.From &&
                x.Date <= firstWeekEnd)
            .ToListAsync(cancellationToken);

        // Group slots by mentor for efficient processing
        var slotsByMentor = allFirstWeekSlots
            .GroupBy(x => x.MentorId)
            .ToDictionary(g => g.Key, g => g.ToList());

        // Process all mentors in parallel for slot generation
        var newSlotsList = mentors
            .AsParallel()
            .Select<User, (User Mentor, IEnumerable<Slot> Slots)>(mentor =>
            {
                // Get first week slots for this mentor
                if (!slotsByMentor.TryGetValue(mentor.Id, out var mentorFirstWeekSlots) ||
                    mentorFirstWeekSlots.Count == 0)
                    return (Mentor: mentor, Slots: Array.Empty<Slot>());

                // Generate new slots for this mentor
                var generatedSlots = GenerateNewSlots(mentorFirstWeekSlots, 10).ToList();
                return (Mentor: mentor, Slots: generatedSlots);
            })
            .Where<(User Mentor, IEnumerable<Slot> Slots)>(result => result.Slots.Any())
            .ToList();

        // Prepare all new slots and update mentors
        var allNewSlots = new List<Slot>();
        foreach (var (mentor, slots) in newSlotsList)
        {
            allNewSlots.AddRange(slots);
            mentor.CreateSlot(slots);
        }

        // Batch insert if the repository supports it
        const int batchSize = 1000;
        for (var i = 0; i < allNewSlots.Count; i += batchSize)
        {
            var batch = allNewSlots
                .Skip(i)
                .Take(batchSize)
                .ToList();

            slotRepository.AddRange(batch);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Success();
    }

    private static List<Slot> GenerateNewSlots(IEnumerable<Slot> firstWeekSlots, int weeks)
    {
        // Convert to array to avoid multiple enumeration
        var slotsArray = firstWeekSlots.ToArray();

        // Pre-calculate the number of slots we'll generate
        var totalSlots = slotsArray.Length * weeks;
        var result = new List<Slot>(totalSlots);

        // Generate slots in a more efficient way
        for (var weekOffset = 1; weekOffset <= weeks; weekOffset++)
        {
            var daysToAdd = weekOffset * 7;

            result.AddRange(from slot in slotsArray
                let newDate = slot.Date.AddDays(daysToAdd)
                select new Slot
                {
                    Id = Guid.NewGuid(),
                    MentorId = slot.MentorId,
                    StartTime = slot.StartTime,
                    EndTime = slot.EndTime,
                    Date = newDate,
                    Note = slot.Note,
                    IsOnline = slot.IsOnline,
                    Month = (short)newDate.Month
                });
        }

        return result;
    }
}