using System.Reflection;

namespace MBS_COMMAND.Contract;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}