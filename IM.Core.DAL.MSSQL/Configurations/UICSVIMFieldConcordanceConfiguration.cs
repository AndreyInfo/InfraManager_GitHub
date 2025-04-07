using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Import.CSV;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class UICSVIMFieldConcordanceConfiguration : UICSVIMFieldConcordanceConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_UICSVIMFieldConcordance";

        protected override string ForeignKeyName => "FK_UICSVIMFieldConcordance_UICSVConfiguration";

        protected override void ConfigureDatabase(EntityTypeBuilder<UICSVIMFieldConcordance> builder)
        {
            builder.ToTable("UICSVIMFieldConcordance", "dbo");

            builder.Property(x => x.CSVConfigurationID)
                .HasColumnName("CSVConfigurationID");
            builder.Property(x => x.IMFieldID)
                .HasColumnName("IMFieldID");
            builder.Property(x => x.Expression)
                .HasColumnName("Expression");
        }
    }
}
