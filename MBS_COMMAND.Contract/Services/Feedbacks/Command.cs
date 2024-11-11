using MBS_COMMAND.Contract.Abstractions.Messages;

namespace MBS_COMMAND.Contract.Services.Feedbacks;

public static class Command
{
   public record CreateFeedback(Guid GroupId, string? Content,Guid ScheduleId, int Rating,bool IsPresent) : ICommand;
}