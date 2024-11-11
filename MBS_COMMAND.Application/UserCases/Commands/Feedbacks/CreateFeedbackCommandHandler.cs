using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Feedbacks;
using MBS_COMMAND.Domain.Abstractions;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Application.UserCases.Commands.Feedbacks;

public class CreateFeedbackCommandHandler(
    IRepositoryBase<Feedback, Guid> feedbackRepository,
    
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork)
    : ICommandHandler<Command.CreateFeedback>
{
    public async Task<Result> Handle(Command.CreateFeedback request, CancellationToken cancellationToken)
    {
        var role = currentUserService.Role == "1";
        var feedback = new Feedback
        {
            Content = request.Content,
            Rating = request.Rating,
            ScheduleId = request.ScheduleId,
            IsMentor = role,
        };
        feedbackRepository.Add(feedback);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}