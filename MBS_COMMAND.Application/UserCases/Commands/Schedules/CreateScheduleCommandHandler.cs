using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Schedule;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Application.UserCases.Commands.Schedules;
public class CreateScheduleCommandHandler : ICommandHandler<Command.CreateScheduleCommand>
{
    private readonly IRepositoryBase<User, Guid> _userRepository;
    private readonly IRepositoryBase<Group, Guid> _groupRepository;
    private readonly IRepositoryBase<Slot, Guid> _slotRepository;
    private readonly IRepositoryBase<Subject, Guid> _subjectRepository;
    private readonly IRepositoryBase<Schedule, Guid> _scheduleRepository;

    public CreateScheduleCommandHandler(IRepositoryBase<User, Guid> userRepository,
        IRepositoryBase<Group, Guid> groupRepository, IRepositoryBase<Slot, Guid> slotRepository,
        IRepositoryBase<Subject, Guid> subjectRepository, IRepositoryBase<Schedule, Guid> scheduleRepository)
    {
        _userRepository = userRepository;
        _groupRepository = groupRepository;
        _slotRepository = slotRepository;
        _subjectRepository = subjectRepository;
        _scheduleRepository = scheduleRepository;
    }

    public async Task<Result> Handle(Command.CreateScheduleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByIdAsync(request.UserId, cancellationToken);

        if (user == null || user.IsDeleted)
        {
            return Result.Failure(new Error("400", "User is not exist !"));
        }
        
        var group = await _groupRepository.FindSingleAsync(x => x.LeaderId.Equals(user.Id) && x.ProjectId.Equals(request.ProjectId), cancellationToken);

        if (group == null || group.IsDeleted)
        {
            return Result.Failure(new Error("403", "Must own a group !"));
        }

        var slot = await _slotRepository.FindByIdAsync(request.SlotId, cancellationToken);

        if (slot == null || group.IsDeleted)
        {
            return Result.Failure(new Error("400", "Slot is not exist !"));
        }

        if (slot.IsBook)
        {
            return Result.Failure(new Error("403", "Slot is booked !"));
        }

        var subject = await _subjectRepository.FindByIdAsync(request.SubjectId, cancellationToken);

        if (subject == null || subject.IsDeleted)
        {
            return Result.Failure(new Error("400", "Subject is not exist !"));
        }

        var start = TimeOnly.TryParse(request.StartTime, out TimeOnly newStart);
        var end = TimeOnly.TryParse(request.EndTime, out TimeOnly newEnd);

        if (newStart.CompareTo(slot.StartTime) < 0 ||
            newEnd.CompareTo(slot.EndTime) > 0)
        {
            return Result.Failure(new Error("500", "Invalid booking time !"));
        }

        if ((newEnd - newStart).TotalHours < 30)
        {
            return Result.Failure(new Error("500", "Booking times must larger than 30 minutes !"));
        }

        var schedule = new Schedule()
        {
            Id = Guid.NewGuid(),
            StartTime = newStart,
            EndTime = newEnd,
            Date = slot.Date,
            MentorId = slot.MentorId ?? new Guid(),
            SubjectId = request.SubjectId,
            GroupId = group.Id,
            SlotId = slot.Id
        };

        slot.IsBook = true;
        slot.ChangeSlotStatusInToBooked(slot.Id);
        _scheduleRepository.Add(schedule);

        return Result.Success("Booking Schedule Successfully !");
    }
}
