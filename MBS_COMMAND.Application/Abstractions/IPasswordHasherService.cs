namespace MBS_COMMAND.Application.Abstractions;

public interface IPasswordHasherService
{
    public string HashPassword(string password);

    public bool VerifyPassword(string password, string hashedPassword);
}