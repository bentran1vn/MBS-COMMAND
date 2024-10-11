using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Slots;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Application.UserCases.Commands.Slots;

public sealed class CreateSlotCommandHandler : ICommandHandler<Command.CreateSlot>
{
    private readonly IRepositoryBase<Slot, Guid> _slotRepository;
    private readonly IRepositoryBase<User, Guid> _userRepository;

    public CreateSlotCommandHandler(IRepositoryBase<Slot, Guid> slotRepository, IRepositoryBase<User, Guid> userRepository)
    {
        _slotRepository = slotRepository;
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(Command.CreateSlot request, CancellationToken cancellationToken)
    {
        var U = await _userRepository.FindByIdAsync(request.MentorId);
        if (U == null)
            return Result.Failure(new Error("404", "User Not Found"));
        foreach (var slotModel in request.SlotModels)
        {

            var Start = TimeOnly.Parse(slotModel.StartTime);
            var End = TimeOnly.Parse(slotModel.EndTime);
            var Date = DateOnly.Parse(slotModel.Date);
            var slot = new Slot
            {
                MentorId = U.Id,
                StartTime = Start,
                EndTime = End,
                Date = Date,
                Note = slotModel.Note,
                IsOnline = slotModel.IsOnline,
                Month = (short?)Date.Month,

            };
            _slotRepository.Add(slot);
        }
        return Result.Success();
    }
}
