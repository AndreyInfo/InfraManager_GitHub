using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Parameters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class ParameterEnumValueConfiguration : ParameterEnumValueConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_parameter_enum_value";

        protected override string ParameterEnumValue_ParameterEnumValue =>
            "fk_parameter_enum_value_parameter_enum_value";

        protected override string ParameterEnumValue_ParameterEnum => "fk_parameter_enum_value_parameter_enum";

        protected override void ConfigureDatabase(EntityTypeBuilder<ParameterEnumValue> builder)
        {
            builder.ToTable("parameter_enum_value", Options.Scheme);
            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("id").IsRequired();
            builder.Property(x => x.Value).HasColumnName("value").IsRequired();
            builder.Property(x => x.OrderPosition).HasColumnName("order_position").IsRequired();
            builder.Property(x => x.ParentID).HasColumnName("parent_id");
            builder.Property(x => x.ParameterEnumID).HasColumnName("parameter_enum_id").IsRequired();

            builder.HasOne(d => d.Parent)
                .WithMany(p => p.ParameterEnums)
                .HasForeignKey(d => d.ParentID)
                .HasConstraintName(ParameterEnumValue_ParameterEnumValue);

            builder.HasOne(d => d.ParameterEnum)
                .WithMany(p => p.ParameterEnumValues)
                .HasForeignKey(d => d.ParameterEnumID)
                .HasConstraintName(ParameterEnumValue_ParameterEnum);
        }
    }
}