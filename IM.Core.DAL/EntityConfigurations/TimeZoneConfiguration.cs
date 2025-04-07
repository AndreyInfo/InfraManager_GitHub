using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class TimeZoneConfiguration : TableConfigurationBase<TimeZone>, IEntityTypeConfiguration<TimeZone>
    {
        protected override void ConfigureCommon(EntityTypeBuilder<TimeZone> builder)
        {
            builder.HasMany(x => x.TimeZoneAdjustmentRules)
                .WithOne()
                .HasForeignKey(x => x.TimeZoneID);
        }
    }
}
