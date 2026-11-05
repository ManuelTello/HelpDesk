using System.Runtime.CompilerServices;
using HelpDesk.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace HelpDesk.DAL.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Ticket> Tickets { get; set; }

    public DbSet<HistoryItem> HistorySummary { get; set; }

    public DbSet<User> Users { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Ticket>(e =>
        {
            e.HasOne(t => t.Creator)
                .WithMany(u => u.Created)
                .HasForeignKey(t => t.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(t => t.Owner)
                .WithMany(u => u.Owned)
                .HasForeignKey(t => t.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
