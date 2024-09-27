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

    // public DbSet<AppUser> AppUses { get; set; }
    // public DbSet<Action> Actions { get; set; }
    // public DbSet<Function> Functions { get; set; }
    // public DbSet<ActionInFunction> ActionInFunctions { get; set; }
    // public DbSet<Permission> Permissions { get; set; }
    //
    // public DbSet<Product> Products { get; set; }
}