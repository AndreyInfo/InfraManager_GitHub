using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    public class UICSVConfiguration : UICSVConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_UICSVConfiguration";

        protected override void ConfigureDatabase(EntityTypeBuilder<Import.CSV.UICSVConfiguration> builder)
        {
            builder.ToTable("UICSVConfiguration", "dbo");

            builder.Property(x => x.ID)
                .HasColumnName("ID");
            builder.Property(x => x.Name)
                .HasColumnName("Name");
            builder.Property(x => x.Note)
                .HasColumnName("Note");
            builder.Property(x => x.Delimiter)
                .HasColumnName("Delimiter");
            builder.Property(x => x.RowVersion)
               .HasColumnName("RowVersion");
        }
    }
}
