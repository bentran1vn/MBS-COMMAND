using MBS_COMMAND.Contract.Abstractions.Messages;
namespace MBS_COMMAND.Contract.Services.Slots;
public static class Command
{
    public record CreateSlot(Guid MentorId, List<SlotModel> SlotModels) : ICommand;

    public record GenerateSlotForSemester(): ICommand;
}
