using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Schedule;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Application.UserCases.Commands.Schedules;

public class DeleteScheduleCommandHandler : ICommandHandler<Command.DeleteScheduleCommand>
{
    private readonly IRepositoryBase<Schedule, Guid> _scheduleRepository;
    private readonly IRepositoryBase<User, Guid> _userRepository;

    public DeleteScheduleCommandHandler(IRepositoryBase<Schedule, Guid> scheduleRepository, IRepositoryBase<User, Guid> userRepository)
    {
        _scheduleRepository = scheduleRepository;
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(Command.DeleteScheduleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByIdAsync(request.UserId, cancellationToken);

         if (user == null || user.IsDeleted)
         {
             return Result.Failure(new Error("400", "User is not exist !"));
         }
        
        var scheduleExist = await _scheduleRepository.FindByIdAsync(request.ScheduleId, cancellationToken, x => x.Group!);
        
        if(scheduleExist is null || scheduleExist.IsDeleted) return Result.Failure(new Error("404", "Schedule not Exist !"));
        
        if (!scheduleExist.Group!.LeaderId.Equals(user.Id))
        {
            return Result.Failure(new Error("401", "Unauthorized To Change Schedule!"));
        }
        
        TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        var now = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, vietnamTimeZone);
        
        DateTime startDateTime = scheduleExist.Date.ToDateTime(scheduleExist.StartTime);
        DateTime endDateTime = scheduleExist.Date.ToDateTime(scheduleExist.EndTime);
        TimeSpan vietnamOffset = TimeSpan.FromHours(7);
        DateTimeOffset startDateTimeOffset = new DateTimeOffset(startDateTime, vietnamOffset);
        DateTimeOffset endDateTimeOffset = new DateTimeOffset(endDateTime, vietnamOffset);

        if (now < startDateTimeOffset && (startDateTimeOffset - now).TotalHours <= 10)
        {
            return Result.Failure(new Error("500", "Cancellation is not allowed within 10 hours of the scheduled start time.")); 
        }

        if (now > endDateTimeOffset)
        {
            return Result.Failure(new Error("500", "Cancellation is not permitted for schedules that have already concluded.")); 
        }
        
        _scheduleRepository.Remove(scheduleExist);

        return Result.Success("Cancellation Schedule Successfully !");
    }
}