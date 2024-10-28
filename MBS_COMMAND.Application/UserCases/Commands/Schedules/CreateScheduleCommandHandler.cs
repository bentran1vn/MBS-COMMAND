using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Schedule;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Application.UserCases.Commands.Schedules;
public class CreateScheduleCommandHandler(
    IRepositoryBase<User, Guid> userRepository,
    IRepositoryBase<Group, Guid> groupRepository,
    IRepositoryBase<Slot, Guid> slotRepository,
    IRepositoryBase<Subject, Guid> subjectRepository,
    IRepositoryBase<Schedule, Guid> scheduleRepository)
    : ICommandHandler<Command.CreateScheduleCommand>
{
    public async Task<Result> Handle(Command.CreateScheduleCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByIdAsync(request.UserId, cancellationToken);

        if (user == null || user.IsDeleted)
        {
            return Result.Failure(new Error("404", "User is not exist !"));
        }

        var group = await groupRepository.FindByIdAsync(request.GroupId, cancellationToken);

        if (group == null || group.IsDeleted || !group.LeaderId.Equals(user.Id))
        {
            return Result.Failure(new Error("403", "Must own a group !"));
        }

        var slot = await slotRepository.FindByIdAsync(request.SlotId, cancellationToken);

        if (slot == null || group.IsDeleted)
        {
            return Result.Failure(new Error("404", "Slot is not exist !"));
        }

        if (slot.IsBook)
        {
            return Result.Failure(new Error("403", "Slot is booked !"));
        }

        var subject = await subjectRepository.FindByIdAsync(request.SubjectId, cancellationToken);

        if (subject == null || subject.IsDeleted)
        {
            return Result.Failure(new Error("404", "Subject is not exist !"));
        }

        var isAccepted = group.MentorId == slot.MentorId;
        var start = TimeOnly.Parse(request.StartTime);
        var end = TimeOnly.Parse(request.EndTime);

        if (start.CompareTo(slot.StartTime) < 0 ||
            end.CompareTo(slot.EndTime) > 0)
        {
            return Result.Failure(new Error("500", "Invalid booking time !"));
        }

        var schedule = new Schedule
        {
            Id = Guid.NewGuid(),
            StartTime = start,
            EndTime = end,
            Date = slot.Date,
            MentorId = slot.MentorId ?? new Guid(),
            SubjectId = request.SubjectId,
            GroupId = group.Id,
            SlotId = slot.Id,
            IsAccepted = isAccepted,
        };

        slot.IsBook = true;
        slot.ChangeSlotStatusInToBooked(slot.Id);
        scheduleRepository.Add(schedule);

        return Result.Success("Booking Schedule Successfully !");
    }
}