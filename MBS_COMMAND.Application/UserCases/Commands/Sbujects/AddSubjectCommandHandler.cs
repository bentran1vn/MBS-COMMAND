using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Subjects;
using MBS_COMMAND.Domain.Abstractions;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Application.UserCases.Commands.Sbujects;

public class AddSubjectCommandHandler(
    IRepositoryBase<Subject, Guid> subjectRepository,
    IRepositoryBase<Semester, Guid> semesterRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<Command.AddSubject>
{

    public async Task<Result> Handle(Command.AddSubject request, CancellationToken cancellationToken)
    {
        var currentSemester = await semesterRepository.FindSingleAsync(x => x.IsActive, cancellationToken);
        if (currentSemester == null)
            return Result.Failure(new Error("404", "Semester Not Found"));
        var isCurrentSubjectExist = subjectRepository.FindAll(x => x.Name == request.Name && x.SemesterId == currentSemester.Id);
        if (isCurrentSubjectExist.Any())
            return Result.Failure(new Error("404", "Subject Already Exist In This Semester"));
        var subject = new Subject
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Status = 1,
            SemesterId = currentSemester.Id
        };
        subjectRepository.Add(subject);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}