namespace HelpDesk.Service.Exceptions;

public class UserInvalidCredentailsException : AppException
{
    public UserInvalidCredentailsException() : base(404, "Invalid username or password.")
    { }
}
