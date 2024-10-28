using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Schedule;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;


namespace MBS_COMMAND.Application.UserCases.Commands.Schedules;

public class UpdateScheduleCommandHandler : ICommandHandler<Command.UpdateScheduleCommand>
{
    private readonly IRepositoryBase<Schedule, Guid> _scheduleRepository;
    private readonly IRepositoryBase<User, Guid> _userRepository;

    public UpdateScheduleCommandHandler(IRepositoryBase<Schedule, Guid> scheduleRepository, IRepositoryBase<User, Guid> userRepository)
    {
        _scheduleRepository = scheduleRepository;
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(Command.UpdateScheduleCommand request, CancellationToken cancellationToken)
    {
        var scheduleExist = await _scheduleRepository.FindByIdAsync(request.ScheduleId, cancellationToken, x => x.Group!);
        
        if(scheduleExist is null || scheduleExist.IsDeleted) return Result.Failure(new Error("404", "Schedule not Exist !"));
        
        TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        var now = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, vietnamTimeZone);

        DateTime dateTime = scheduleExist.Date.ToDateTime(scheduleExist.StartTime);
        TimeSpan vietnamOffset = TimeSpan.FromHours(7);
        DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime, vietnamOffset);
        
        var user = await _userRepository.FindByIdAsync(request.UserId, cancellationToken);

        if (user == null || user.IsDeleted)
        {
            return Result.Failure(new Error("400", "User is not exist !"));
        }

        if (!scheduleExist.Group!.LeaderId.Equals(user.Id))
        {
            return Result.Failure(new Error("401", "Unauthorized To Change Schedule!"));
        }
        
        if (dateTimeOffset > now && (dateTimeOffset - now).TotalHours > 10)
        {
            var start = TimeOnly.TryParse(request.StartTime, out TimeOnly newStart);
            var end = TimeOnly.TryParse(request.EndTime, out TimeOnly newEnd);
            
            if (!start || newStart < scheduleExist.StartTime)
            {
                return Result.Failure(new Error("500", "New StartTime must be valid!")); 
            }
            if (!end || newEnd > scheduleExist.EndTime)
            {
                return Result.Failure(new Error("500", "New EndTime must be valid!")); 
            }
            
            if ((newEnd - newStart).TotalHours < 30)
            {
                return Result.Failure(new Error("500", "Booking times must larger than 30 minutes !"));
            }

            scheduleExist.StartTime = newStart;
            scheduleExist.EndTime = newEnd;
            
            _scheduleRepository.Update(scheduleExist);
            
            return Result.Success("Update Schedules Successfully !");
        }
            
        return Result.Failure(new Error("500", "Fail to Update Schedule !"));
    }
}