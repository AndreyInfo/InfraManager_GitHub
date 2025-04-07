using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class TimeZoneAdjustmentRuleConfiguration : TableConfigurationBase<TimeZoneAdjustmentRule>, IEntityTypeConfiguration<TimeZoneAdjustmentRule>
    {
        protected override void ConfigureCommon(EntityTypeBuilder<TimeZoneAdjustmentRule> builder)
        {
            builder.HasKey(x => new { x.TimeZoneID, x.DateStart, x.DateEnd });
        }
    }
}
