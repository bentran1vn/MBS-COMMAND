using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Mentors;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Application.UserCases.Commands.Users;

public sealed class CreateMentorCommandHandler : ICommandHandler<Command.CreateMentorCommand>
{
    private readonly IRepositoryBase<User, Guid> _repository;

    public CreateMentorCommandHandler(IRepositoryBase<User, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(Command.CreateMentorCommand request, CancellationToken cancellationToken)
    {
        var Mentor = await _repository.FindByIdAsync(request.MentorId, cancellationToken);
        if (Mentor == null)
        {
            return Result.Failure(new Error("404", "Mentor Not Existed !"));
        }
        Mentor.Role = 1;
        Mentor.Status = 1;
        Mentor.CreateMentor(Mentor);
        return Result.Success();

    }
}
