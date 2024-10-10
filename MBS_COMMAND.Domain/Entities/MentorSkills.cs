using MBS_COMMAND.Domain.Abstractions.Aggregates;
using MBS_COMMAND.Domain.Abstractions.Entities;
using DomainEventShared = MBS_CONTRACT.SHARE.Services.Mentors.DomainEvent;

namespace MBS_COMMAND.Domain.Entities;

public class MentorSkills : AggregateRoot<Guid>, IAuditableEntity
{
    public Guid SkillId { get; set; }
    public virtual Skill Skill { get; set; } = default!;
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = default!;
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }
    public virtual IReadOnlyCollection<Certificate> CertificateList { get; set; } = default!;
    
    public void CreateMentorSkills(Guid mentorId, Skill skill, List<Certificate> cerificates)
    {
        var domainSkill = new MBS_CONTRACT.SHARE.Services.MentorSkills.DomainEvent.Skill()
        {
            Name = skill.Name,
            Description = skill.Description,
            CateogoryType = skill.Category.Name,
            CreatedOnUtc = skill.CreatedOnUtc
        };
    
        var domainCertificates = cerificates.Select(x => new MBS_CONTRACT.SHARE.Services.MentorSkills.DomainEvent.Certificate
        {
            Name = x.Name,
            Description = x.Description,
            ImageUrl = x.ImageUrl
        }).ToList();
        
        RaiseDomainEvent(new MBS_CONTRACT.SHARE.Services.MentorSkills.DomainEvent.MentorSkillsCreated(Guid.NewGuid(), Id, mentorId, domainSkill, domainCertificates));
    }
    
    public void CreateMentor(User user)
    {
        RaiseDomainEvent( new DomainEventShared.MentorCreated(
                Guid.NewGuid(), user.Id, user.Email,
                user.FullName ?? "", user.Role, user.Points,
                user.Status, user.CreatedOnUtc, user.IsDeleted));
    }
}