using System.Transactions;
using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Schedule;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;
using MBS_COMMAND.Persistence;
using Microsoft.EntityFrameworkCore;
using Transaction = MBS_COMMAND.Domain.Entities.Transaction;

namespace MBS_COMMAND.Application.UserCases.Commands.Schedules;
public class CreateScheduleCommandHandler(
    IRepositoryBase<User, Guid> userRepository,
    IRepositoryBase<Group, Guid> groupRepository,
    IRepositoryBase<Slot, Guid> slotRepository,
    IRepositoryBase<Subject, Guid> subjectRepository,
    IRepositoryBase<Schedule, Guid> scheduleRepository,
    ApplicationDbContext dbContext,
    IRepositoryBase<Transaction, Guid> transactionRepository)
    : ICommandHandler<Command.CreateScheduleCommand>
{
    public async Task<Result> Handle(Command.CreateScheduleCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByIdAsync(request.UserId, cancellationToken);

        if (user == null || user.IsDeleted)
        {
            return Result.Failure(new Error("404", "User is not exist !"));
        }

        var group = await groupRepository.FindByIdAsync(request.GroupId, cancellationToken, x => x.Members);

        if (group == null || group.IsDeleted || !group.LeaderId.Equals(user.Id))
        {
            return Result.Failure(new Error("403", "Must own a group !"));
        }

        if (group.Members == null || !group.Members.Any() || group.Members.Count < 3)
        {
            return Result.Failure(new Error("500", "Your group must have at least 3 members !"));
        }

        if (group.Project is null)
        {
            return Result.Failure(new Error("500", "Your group don't have a project. !"));
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

        var isAccepted = group.MentorId == slot.MentorId? 1 : 0;
        var start = TimeOnly.Parse(request.StartTime);
        var end = TimeOnly.Parse(request.EndTime);
        
        
        var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        var now = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, vietnamTimeZone);
        var endTimeDatetime = slot.Date.ToDateTime(slot.EndTime);
        
        if (endTimeDatetime < now.DateTime)
        {
            return Result.Failure(new Error("500", "Can not book the old schedule !"));
        }
            
        if (start.CompareTo(slot.StartTime) < 0 ||
                end.CompareTo(slot.EndTime) > 0)
        {
            return Result.Failure(new Error("500", "Invalid booking time !"));
        }
        
        if ((end - start).TotalMinutes < 30)
        {
            return Result.Failure(new Error("500", "Booking times must larger than 30 minutes !"));
        }

        var point = await dbContext.Configs.SingleOrDefaultAsync(x => x.Key.Equals("BookingPoints"), cancellationToken);

        if (point == null)
        {
            return Result.Failure(new Error("500", "Booking points is not exist !"));
        }

        if (group.BookingPoints < point!.Value)
        {
            return Result.Failure(new Error("500", "Not enough points to book"));
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
        if (isAccepted == 1)
        {
            group.BookingPoints -= point.Value;
        }
       

        var transactions = group.Members.Select(x => new Transaction()
        {
            UserId = x.StudentId,
            ScheduleId = schedule.Id,
            Date = schedule.Date,
            Point = point.Value / group.Members.Count,
            Status = 0
        }).ToList();
        
        transactionRepository.AddRange(transactions);

        return Result.Success("Booking Schedule Successfully !");
    }
}