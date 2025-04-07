using Core = InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class TimeZoneConfiguration : Core.TimeZoneConfiguration
    {
        protected override string TableName => "TimeZone";

        protected override string TableSchema => "dbo";

        protected override void ConfigureDbProvider(EntityTypeBuilder<ServiceDesk.TimeZone> builder)
        {
            builder.HasKey(e => e.ID);

            builder.Property(x => x.ID)
                .HasColumnName("ID")
                .HasMaxLength(250);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(250)
                .HasColumnName("Name");

            builder.Property(x => x.BaseUtcOffsetInMinutes)
                .IsRequired()
                .HasColumnType("smallint")
                .HasColumnName("BaseUtcOffsetInMinutes");

            builder.Property(x => x.SupportsDaylightSavingTime)
                .IsRequired()
                .HasColumnName("SupportsDaylightSavingTime");

            builder.HasMany(x => x.TimeZoneAdjustmentRules)
                .WithOne()
                .HasForeignKey(x => x.TimeZoneID);
        }
    }
}
