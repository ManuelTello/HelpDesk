namespace HelpDesk.Service.Exceptions;

public class AppException : Exception
{
    public int Code { get; init; }

    public AppException(int code, string message) : base(message)
    {
        Code = code;
    }
}
