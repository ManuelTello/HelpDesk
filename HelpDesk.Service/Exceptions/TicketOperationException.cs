namespace HelpDesk.Service.Exceptions;

public class TicketOperationException : AppException
{
    public TicketOperationException() : base(409, "The state of the ticket is invalid for this operation.")
    { }
}
