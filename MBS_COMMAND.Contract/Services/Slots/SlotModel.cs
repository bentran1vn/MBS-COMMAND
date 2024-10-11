namespace MBS_COMMAND.Contract.Services.Slots;

public class SlotModel
{
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string Date { get; set; }

    public bool IsOnline { get; set; }
    public string? Note { get; set; }

}
