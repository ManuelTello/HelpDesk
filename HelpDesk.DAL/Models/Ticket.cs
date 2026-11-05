using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.DAL.Models;

public class Ticket
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid CreatorId { get; set; }

    public User Creator { get; set; } = null!;

    public User? Owner { get; set; } = null!;

    public Guid? OwnerId { get; set; }

    public string Subject { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ICollection<HistoryItem> HistorySummary { get; } = [];

    public TicketCategory Category { get; set; }

    public TicketPriority Priority { get; set; }

    public DateTime DateTimeCreated { get; set; }

    public DateTime? DateTimeClosed { get; set; }
}