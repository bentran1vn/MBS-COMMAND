namespace MBS_COMMAND.Domain.Exceptions;

public abstract class NotFoundException : DomainException
{
    protected NotFoundException(string message)
        : base("Not Found", message)
    {
    }
}
