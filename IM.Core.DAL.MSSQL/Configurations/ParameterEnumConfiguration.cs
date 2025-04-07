using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Parameters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class ParameterEnumConfiguration : ParameterEnumConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_ParameterEnum";
        protected override string UI_Name => "UI_ParameterEnum_Name";

        protected override void ConfigureDatabase(EntityTypeBuilder<ParameterEnum> builder)
        {
            builder.ToTable("ParameterEnum", "dbo");

            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.IsTree).HasColumnName("IsTree");
            builder.Property(x => x.RowVersion).HasColumnName("RowVersion").HasColumnType("timestamp").IsRowVersion();
        }
    }
}
