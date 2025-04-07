using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Parameters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class ParameterEnumValueConfiguration : ParameterEnumValueConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_ParameterEnumValue";
        protected override string ParameterEnumValue_ParameterEnumValue => "FK_ParameterEnumValue_ParameterEnumValue";
        protected override string ParameterEnumValue_ParameterEnum => "FK_ParameterEnumValue_ParameterEnum";
        protected override void ConfigureDatabase(EntityTypeBuilder<ParameterEnumValue> builder)
        {
            builder.ToTable("ParameterEnumValue", "dbo");

            builder.Property(x => x.ID).HasColumnName("ID").IsRequired();
            builder.Property(x => x.Value).HasColumnName("Value").IsRequired();
            builder.Property(x => x.OrderPosition).HasColumnName("OrderPosition").IsRequired();
            builder.Property(x => x.ParentID).HasColumnName("ParentID");
            builder.Property(x => x.ParameterEnumID).HasColumnName("ParameterEnumID").IsRequired();

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
