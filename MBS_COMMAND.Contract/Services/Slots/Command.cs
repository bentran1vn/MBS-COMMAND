﻿using MBS_COMMAND.Contract.Abstractions.Messages;
namespace MBS_COMMAND.Contract.Services.Slots;
public class Command
{
    public record CreateSlot(Guid MentorId, List<SlotModel> SlotModels) : ICommand;
}