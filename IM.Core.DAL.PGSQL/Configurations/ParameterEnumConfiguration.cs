using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Parameters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class ParameterEnumConfiguration : ParameterEnumConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_parameter_enum";

        protected override string UI_Name => "ui_parameter_enum_name";

        protected override void ConfigureDatabase(EntityTypeBuilder<ParameterEnum> builder)
        {
            builder.ToTable("parameter_enum", Options.Scheme);
            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(x => x.IsTree).HasColumnName("is_tree");
            builder.HasXminRowVersion(x => x.RowVersion);
        }
    }
}