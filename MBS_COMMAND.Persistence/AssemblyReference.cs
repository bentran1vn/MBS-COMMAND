using System.Reflection;

namespace MBS_COMMAND.Persistence;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}