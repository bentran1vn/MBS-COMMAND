using MBS_COMMAND.Contract.Abstractions.Messages;

namespace MBS_COMMAND.Contract.Services.Slots;
public static class Command
{
    public record CreateSlot(List<SlotModel> SlotModels) : ICommand;

    public record GenerateSlotForSemester : ICommand;

    public record UpdateSlot(SlotModel SlotModel) : ICommand;

    public record DeleteSlot(Guid Id) : ICommand;
}