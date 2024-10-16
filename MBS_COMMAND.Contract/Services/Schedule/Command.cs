using System.ComponentModel;
using MBS_COMMAND.Contract.Abstractions.Messages;
using Swashbuckle.AspNetCore.Annotations;

namespace MBS_COMMAND.Contract.Services.Schedule;

public class Command
{
    public record CreateScheduleCommand : ICommand
    {
        [SwaggerSchema(ReadOnly = true)]
        [DefaultValue("e824c924-e441-4367-a03b-8dd13223f76f")]
        public Guid UserId { get; set; }
        
        public Guid SlotId { get; set; }
        public Guid SubjectId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}