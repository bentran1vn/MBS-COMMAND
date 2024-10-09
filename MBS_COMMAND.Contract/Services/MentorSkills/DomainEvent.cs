using MBS_COMMAND.Contract.Abstractions.Messages;

namespace MBS_COMMAND.Contract.Services.MentorSkills;

public class DomainEvent
{
    public record MentorSkillsCreated(Guid IdEvent, Guid Id, Guid MentorId,
        Skill skill, IReadOnlyCollection<Certificate> Certificates) : IDomainEvent, ICommand;
    
    public class Skill
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CateogoryType { get; set; }
        public DateTimeOffset CreatedOnUtc { get; set; }
        public DateTimeOffset? ModifiedOnUtc { get; set; }
    }

    public class Certificate
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}