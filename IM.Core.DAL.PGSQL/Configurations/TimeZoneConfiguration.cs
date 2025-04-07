using IM.Core.DAL.Postgres;
using Core = InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class TimeZoneConfiguration : Core.TimeZoneConfiguration
    {
        protected override string TableName => "time_zone";

        protected override string TableSchema => Options.Scheme;

        protected override void ConfigureDbProvider(EntityTypeBuilder<ServiceDesk.TimeZone> builder)
        {
            builder.HasKey(e => e.ID).HasName("pk_time_zone");

            builder.Property(x => x.ID)
                .HasColumnName("id")
                .HasMaxLength(250);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(250)
                .HasColumnName("name");

            builder.Property(x => x.BaseUtcOffsetInMinutes)
                .IsRequired()
                .HasColumnType("smallint")
                .HasColumnName("base_utc_offset_in_minutes");

            builder.Property(x => x.SupportsDaylightSavingTime)
                .IsRequired()
                .HasColumnName("supports_daylight_saving_time");
        }
    }
}