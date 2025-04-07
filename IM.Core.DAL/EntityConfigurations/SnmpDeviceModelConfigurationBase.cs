using InfraManager.DAL.Snmp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class SnmpDeviceModelConfigurationBase : IEntityTypeConfiguration<SnmpDeviceModel>
    {
        public void Configure(EntityTypeBuilder<SnmpDeviceModel> builder)
        {
            builder.HasKey(m => m.ID).HasName(PrimaryKeyName);
            builder.Property(m => m.OID).IsRequired(false).HasMaxLength(250);
            builder.Property(m => m.OIDValue).IsRequired(false).HasMaxLength(1000);
            builder.Property(m => m.SysObjectIDValue).IsRequired(false).HasMaxLength(1000);
            builder.Property(m => m.DescriptionTag).IsRequired(false).HasMaxLength(1000);
            builder.Property(m => m.ProfileID).IsRequired(false).HasMaxLength(36);
            builder.Property(m => m.ModelName).IsRequired(true).HasMaxLength(1000);
            builder.Property(m => m.ModelID).IsRequired(true);
            builder.Property(m => m.ManufacturerName).IsRequired(false).HasMaxLength(250);
            builder.Property(m => m.Template).IsRequired(true);
            builder.Property(m => m.Note).IsRequired(false).HasMaxLength(1000);
            builder.Property(m => m.IsIgnore).IsRequired(true);
            builder.Property(m => m.IsSynchronized).IsRequired(true);

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<SnmpDeviceModel> builder);

        protected abstract string PrimaryKeyName { get; }
    }
}
