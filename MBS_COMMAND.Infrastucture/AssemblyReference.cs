using System.Reflection;

namespace MBS_COMMAND.Infrastucture;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}