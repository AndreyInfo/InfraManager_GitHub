using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Snmp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations
{
    internal class SnmpDeviceProfileConfiguration : SnmpDeviceProfileConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_SnmpDeviceProfiles";

        protected override void ConfigureDatabase(EntityTypeBuilder<SnmpDeviceProfile> builder)
        {
            builder.ToTable("SnmpDeviceProfiles", Options.Scheme);

            builder.Property(m => m.ID).HasColumnName("ID").HasColumnType("UniqueIdentifier");
            builder.Property(m => m.Name).HasColumnName("[Name]");
            builder.Property(m => m.IsSynchronized).HasColumnName("IsSynchronized");
            builder.Property(m => m.RowVersion).IsRowVersion();
        }
    }
}
