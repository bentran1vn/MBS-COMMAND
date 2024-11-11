using System.ComponentModel;
using MBS_COMMAND.Contract.Abstractions.Messages;
using Swashbuckle.AspNetCore.Annotations;

namespace MBS_COMMAND.Contract.Services.Schedule;

public static class Command
{
    public record CreateScheduleCommand : ICommand
    {
        [SwaggerSchema(ReadOnly = true)]
        [DefaultValue("e824c924-e441-4367-a03b-8dd13223f76f")]
        public Guid UserId { get; set; }
        
        public Guid SlotId { get; set; }
        public Guid GroupId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
    
    public record UpdateScheduleCommand : ICommand
    {
        [SwaggerSchema(ReadOnly = true)]
        [DefaultValue("e824c924-e441-4367-a03b-8dd13223f76f")]
        public Guid UserId { get; set; }
        public Guid ScheduleId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
    
    public record DeleteScheduleCommand : ICommand
    {
        [SwaggerSchema(ReadOnly = true)]
        [DefaultValue("e824c924-e441-4367-a03b-8dd13223f76f")]
        public Guid UserId { get; set; }
        public Guid ScheduleId { get; set; }
    }
    
    public record AcceptScheduleCommand(Guid ScheduleId,int Status) : ICommand;
}