using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => new { t.Id, t.Value });
            builder
                .Property(t => t.WeatherTariff)
                .HasConversion(
                    v => (int)v,
                    v => (WeatherTariff)v
                );
        }
    }
}
