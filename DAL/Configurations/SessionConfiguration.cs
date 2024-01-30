using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.HasKey(s => s.UserId);
            builder
                .Property(s => s.UserId)
                .ValueGeneratedNever();
            builder
                .Property(s => s.WeatherTariff)
                .HasConversion(
                    v => (int)v,
                    v => (WeatherTariff)v
                );
        }
    }
}
