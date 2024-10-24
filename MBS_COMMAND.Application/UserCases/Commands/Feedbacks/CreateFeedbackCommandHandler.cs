using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Feedbacks;
using MBS_COMMAND.Domain.Abstractions;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Application.UserCases.Commands.Feedbacks;

public class CreateFeedbackCommandHandler : ICommandHandler<Command.CreateFeedback>
{
    private readonly IRepositoryBase<Feedback, Guid> _feedbackRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;
    public CreateFeedbackCommandHandler(IRepositoryBase<Feedback, Guid> feedbackRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
    {
        _feedbackRepository = feedbackRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(Command.CreateFeedback request, CancellationToken cancellationToken)
    {
        var role = _currentUserService.Role == "1";
        var feedback = new Feedback
        {
            Content = request.Content,
            GroupId = request.GroupId,
            Rating = request.Rating,
            ScheduleId = request.ScheduleId,
            IsMentor = role,
        };
        _feedbackRepository.Add(feedback);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}