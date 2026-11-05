namespace HelpDesk.Service.Exceptions;

public class UserNameAlreadyTaken : AppException
{
    public UserNameAlreadyTaken(string username) : base(409, $"Username '{username}' already taken.")
    { }
}
