using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.Snmp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations
{
    internal class SnmpDeviceProfileConfiguration : SnmpDeviceProfileConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_snmp_device_profiles";

        protected override void ConfigureDatabase(EntityTypeBuilder<SnmpDeviceProfile> builder)
        {
            builder.ToTable("snmp_device_profiles", Options.Scheme);

            builder.Property(m => m.ID).HasColumnName("id").HasColumnType("uuid");
            builder.Property(m => m.Name).HasColumnName("name");
            builder.Property(m => m.IsSynchronized).HasColumnName("is_synchronized");
            builder.HasXminRowVersion(m => m.RowVersion);
        }
    }
}
