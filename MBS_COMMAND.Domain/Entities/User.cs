﻿using MBS_COMMAND.Domain.Abstractions.Aggregates;
using MBS_COMMAND.Domain.Abstractions.Entities;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Domain.Entities;

public class User : AggregateRoot<Guid>, IAuditableEntity
{
    public string Email { get; set; }
    public string? FullName { get; set; }
    public string Password { get; set; }
    public int Role { get; set; }
    public int Points { get; set; }
    public int Status { get; set; } //0 Not Active, 1 Active, 2 Blocked
    public Guid? MentorId { get; set; }
    public bool IsFirstLogin { get; set; } = true;
    public virtual User? Mentor { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    public virtual ICollection<Group_Student_Mapping>? Groups { get; set; } = [];



    public virtual IReadOnlyCollection<MentorSkills> MentorSkillsList { get; set; } = default!;
}