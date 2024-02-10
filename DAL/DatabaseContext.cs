using Core.Entities;
using Core.Enums;
using DAL.Configurations;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Infrastructure.Helpers;

namespace DAL;

public class DatabaseContext : DbContext
{
    public DbSet<Session> Sessions { get; set; } = null!;
    public DbSet<Ticket> Tickets { get; set; } = null!;

    private readonly AppSettings appSettings;

    public DatabaseContext(IOptions<AppSettings> appSettings)
    {
        this.appSettings = appSettings.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite(appSettings.ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new SessionConfiguration().Configure(modelBuilder.Entity<Session>());
        new TicketConfiguration().Configure(modelBuilder.Entity<Ticket>());
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Added ||
                e.State == EntityState.Modified
            ));

        foreach (var entityEntry in entries)
        {
            ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
                ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
