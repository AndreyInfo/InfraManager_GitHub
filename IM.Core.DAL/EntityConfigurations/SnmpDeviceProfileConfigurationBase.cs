using InfraManager.DAL.Snmp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class SnmpDeviceProfileConfigurationBase : IEntityTypeConfiguration<SnmpDeviceProfile>
    {
        public void Configure(EntityTypeBuilder<SnmpDeviceProfile> builder)
        {
            builder.HasKey(m => m.ID).HasName(PrimaryKeyName);
            builder.Property(m => m.Name).IsRequired(true).HasMaxLength(1000);
            builder.Property(m => m.IsSynchronized).IsRequired(true);

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<SnmpDeviceProfile> builder);

        protected abstract string PrimaryKeyName { get; }
    }
}
