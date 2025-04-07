using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Snmp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations
{
    internal class SnmpDeviceModelConfiguration : SnmpDeviceModelConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_SnmpDeviceModels";

        protected override void ConfigureDatabase(EntityTypeBuilder<SnmpDeviceModel> builder)
        {
            builder.ToTable("SnmpDeviceModels", Options.Scheme);

            builder.Property(m => m.ID).HasColumnName("ID").HasColumnType("UniqueIdentifier");
            builder.Property(m => m.OID).HasColumnName("OID");
            builder.Property(m => m.OIDValue).HasColumnName("OIDValue");
            builder.Property(m => m.SysObjectIDValue).HasColumnName("SysObjectIDValue");
            builder.Property(m => m.DescriptionTag).HasColumnName("DescriptionTag");
            builder.Property(m => m.ProfileID).HasColumnName("ProfileID").HasColumnType("UniqueIdentifier");
            builder.Property(m => m.ModelName).HasColumnName("ModelName");
            builder.Property(m => m.ModelID).HasColumnName("ModelID");
            builder.Property(m => m.ManufacturerName).HasColumnName("ManufacturerName");
            builder.Property(m => m.Template).HasColumnName("Template");
            builder.Property(m => m.Note).HasColumnName("Note");
            builder.Property(m => m.IsIgnore).HasColumnName("IsIgnore");
            builder.Property(m => m.IsSynchronized).HasColumnName("IsSynchronized");
            builder.Property(m => m.RowVersion).IsRowVersion();
        }
    }
}
