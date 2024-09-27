using MBS_AUTHORIZATION.Domain.Entities;
using MBS_COMMAND.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MBS_COMMAND.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder) =>
        builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);

    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Certificate> Certificates { get; set; }
    public virtual DbSet<Config> Configs { get; set; }
    public virtual DbSet<Feedback> Feedbacks { get; set; }
    public virtual DbSet<Group> Groups { get; set; }
    public virtual DbSet<Group_Student_Mapping> group_Student_Mappings { get; set; }
    public virtual DbSet<Project> Projects { get; set; }
    public virtual DbSet<Schedule> Schedules { get; set; }
    public virtual DbSet<Semester> Semesters { get; set; }
    public virtual DbSet<Skill> Skills { get; set; }
    public virtual DbSet<Slot> Slots { get; set; }
    public virtual DbSet<Subject> Subjects { get; set; }
    public virtual DbSet<Transaction> Transactions { get; set; }
    public virtual DbSet<User> Users { get; set; }

}