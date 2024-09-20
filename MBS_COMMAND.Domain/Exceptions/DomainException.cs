namespace MBS_COMMAND.Domain.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException(string title, string message)
        : base(message) =>
        Title = title;

    public string Title { get; }
}