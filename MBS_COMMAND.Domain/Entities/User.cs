using MBS_COMMAND.Domain.Abstractions.Entities;
using MBS_COMMAND.Domain.Entities;

namespace MBS_AUTHORIZATION.Domain.Entities;

public class User : Entity<Guid>, IAuditableEntity
{
    public string Email { get; set; }
    public string? FullName { get; set; }
    public string Password { get; set; }
    public int Role { get; set; }
    public int Points { get; set; }
    public int Status { get; set; }
    public Guid? MentorId { get; set; }
    public bool IsFirstLogin { get; set; } = true;
    public virtual User? Mentor { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }
    public virtual IReadOnlyCollection<MentorSkills> MentorSkillsList { get; set; } = default!;
}