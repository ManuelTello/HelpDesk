namespace HelpDesk.Service.Exceptions;

public class TicketNotFoundException : AppException
{
    public TicketNotFoundException() : base(404, "Ticket not found.")
    {
    }
}
