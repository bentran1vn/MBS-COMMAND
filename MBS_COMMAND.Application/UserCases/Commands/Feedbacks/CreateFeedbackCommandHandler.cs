using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Feedbacks;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Application.UserCases.Commands.Feedbacks;

public class CreateFeedbackCommandHandler : ICommandHandler<Command.CreateFeedback>
{
    private readonly IRepositoryBase<Feedback, Guid> _feedbackRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateFeedbackCommandHandler(IRepositoryBase<Feedback, Guid> feedbackRepository,
        ICurrentUserService currentUserService)
    {
        _feedbackRepository = feedbackRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(Command.CreateFeedback request, CancellationToken cancellationToken)
    {
        var role = _currentUserService.Role == "2";
        var feedback = new Feedback
        {
            Content = request.Content,
            GroupId = request.GroupId,
            Rating = request.Rating,
            ScheduleId = request.ScheduleId,
            IsMentor = role,
        };
        _feedbackRepository.Add(feedback);
        return Result.Success();
    }
}