using FluentValidation;

namespace MBS_COMMAND.Contract.Services.Schedule.Validators;

public class CreateScheduleValidators : AbstractValidator<Command.CreateScheduleCommand>
{
    public CreateScheduleValidators()
    {
        RuleFor(x => x.StartTime).NotEmpty().LessThan(x => x.EndTime);
        RuleFor(x => x.EndTime).NotEmpty().GreaterThan(x => x.StartTime);
        RuleFor(x => x.SlotId).NotEmpty();
        RuleFor(x => x.SubjectId).NotEmpty();
    }
}