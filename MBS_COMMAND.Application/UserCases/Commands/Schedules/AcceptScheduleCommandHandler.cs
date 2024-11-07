using MBS_COMMAND.Application.Abstractions;
using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Schedule;
using MBS_COMMAND.Domain.Abstractions;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Application.UserCases.Commands.Schedules;
public class AcceptScheduleCommandHandler : ICommandHandler<Command.AcceptScheduleCommand>
{
    private readonly IRepositoryBase<Schedule, Guid> _scheduleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMailService _mailService;

    public AcceptScheduleCommandHandler(IRepositoryBase<Schedule, Guid> scheduleRepository, IUnitOfWork unitOfWork,
        IMailService mailService)
    {
        _scheduleRepository = scheduleRepository;
        _unitOfWork = unitOfWork;
        _mailService = mailService;
    }

    public async Task<Result> Handle(Command.AcceptScheduleCommand request, CancellationToken cancellationToken)
    {
        var slot = await _scheduleRepository.FindByIdAsync(request.ScheduleId, cancellationToken);
        if (slot == null)
        {
            return Result.Failure(new Error("404", "Schedule not found"));
        }

        slot.IsAccepted = request.Status;
        _scheduleRepository.Update(slot);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _mailService.SendMail(new MailContent
        {
            To = slot.Group!.Leader!.Email,
            Subject = $"Schedule {slot.Date} {slot.StartTime} - {slot.EndTime}",
            Body = $@" 
                <p>Your schedule has been {(request.Status == 1 ? "accepted":"rejected")}</p>"
        });
        return Result.Success();
    }
}