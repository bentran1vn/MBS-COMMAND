using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Slots;
using MBS_COMMAND.Domain.Abstractions;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;
using static MBS_COMMAND.Contract.Abstractions.Shared.Result;

namespace MBS_COMMAND.Application.UserCases.Commands.Slots;

public class GenerateSlotForSemesterCommandHandler(
    IRepositoryBase<Slot, Guid> slotRepository,
    IRepositoryBase<Semester, Guid> semesterRepository,
    IRepositoryBase<User, Guid> userRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<Command.GenerateSlotForSemester>
{
    public async Task<Result> Handle(Command.GenerateSlotForSemester request, CancellationToken cancellationToken)
    {
        var currentSemester = await semesterRepository.FindSingleAsync(x => x.IsActive, cancellationToken);
        if (currentSemester == null)
            return Result.Failure(new Error("404", "No active semester found"));

        var mentors = userRepository.FindAll(x => x.Role == 2);
        var newSlots = new List<Slot>();

        foreach (var mentor in mentors)
        {
            var firstWeekSlots = slotRepository.FindAll(x =>
                x.MentorId == mentor.Id && x.Date >= currentSemester.From &&
                x.Date <= currentSemester.From.AddDays(6));

            newSlots.AddRange(GenerateNewSlots(firstWeekSlots, 10));
        }

        slotRepository.AddRange(newSlots);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Success();
    }

    private static IEnumerable<Slot> GenerateNewSlots(IEnumerable<Slot> firstWeekSlots, int weeks)
    {
        return Enumerable.Range(1, weeks)
            .SelectMany(i => firstWeekSlots.Select(x => new Slot
            {
                Id = Guid.NewGuid(),
                MentorId = x.MentorId,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                Date = x.Date.AddDays(7 * i),
                Note = x.Note,
                IsOnline = x.IsOnline,
                Month = (short)x.Date.AddDays(7 * i).Month
            }));
    }
}