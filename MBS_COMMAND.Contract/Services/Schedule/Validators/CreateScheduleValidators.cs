using FluentValidation;

namespace MBS_COMMAND.Contract.Services.Schedule.Validators;

public class CreateScheduleValidators : AbstractValidator<Command.CreateScheduleCommand>
{
    public CreateScheduleValidators()
    {
        RuleFor(x => x.StartTime)
            .NotEmpty()
            .Must(BeValidTimeOnly).WithMessage("StartTime must be in a valid TimeOnly format (e.g., HH:mm).")
            .Must((model, startTime) => BeLessThanEndTime(startTime, model.EndTime))
            .WithMessage("StartTime must be less than EndTime.");

        RuleFor(x => x.EndTime)
            .NotEmpty()
            .Must(BeValidTimeOnly).WithMessage("EndTime must be in a valid TimeOnly format (e.g., HH:mm).")
            .Must((model, endTime) => BeGreaterThanStartTime(model.StartTime, endTime))
            .WithMessage("EndTime must be greater than StartTime.");

        RuleFor(x => x.SlotId).NotEmpty();
    }

    // Helper method to check if the string is a valid TimeOnly format
    private bool BeValidTimeOnly(string timeString)
    {
        return TimeOnly.TryParse(timeString, out _);
    }

    // Helper method to check if StartTime < EndTime
    private bool BeLessThanEndTime(string startTime, string endTime)
    {
        if (TimeOnly.TryParse(startTime, out var start) && TimeOnly.TryParse(endTime, out var end))
        {
            return start < end;
        }
        return false;
    }

    // Helper method to check if EndTime > StartTime
    private bool BeGreaterThanStartTime(string startTime, string endTime)
    {
        if (TimeOnly.TryParse(startTime, out var start) && TimeOnly.TryParse(endTime, out var end))
        {
            return end > start;
        }
        return false;
    }
}