using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.Snmp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations
{
    internal class SnmpDeviceModelConfiguration : SnmpDeviceModelConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_snmp_device_models";

        protected override void ConfigureDatabase(EntityTypeBuilder<SnmpDeviceModel> builder)
        {
            builder.ToTable("snmp_device_models", Options.Scheme);

            builder.Property(m => m.ID).HasColumnName("id").HasColumnType("uuid");
            builder.Property(m => m.OID).HasColumnName("o_id");
            builder.Property(m => m.OIDValue).HasColumnName("o_id_value");
            builder.Property(m => m.SysObjectIDValue).HasColumnName("sys_object_id_value");
            builder.Property(m => m.DescriptionTag).HasColumnName("description_tag");
            builder.Property(m => m.ProfileID).HasColumnName("profile_id").HasColumnType("uuid");
            builder.Property(m => m.ModelName).HasColumnName("model_name");
            builder.Property(m => m.ModelID).HasColumnName("model_id");
            builder.Property(m => m.ManufacturerName).HasColumnName("manufacturer_name");
            builder.Property(m => m.Template).HasColumnName("template");
            builder.Property(m => m.Note).HasColumnName("note");
            builder.Property(m => m.IsIgnore).HasColumnName("is_ignore");
            builder.Property(m => m.IsSynchronized).HasColumnName("is_synchronized");
            builder.HasXminRowVersion(m => m.RowVersion);
        }
    }
}
