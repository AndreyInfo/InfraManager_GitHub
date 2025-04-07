using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.EntityConfigurations;

namespace InfraManager.DAL.Postgres.Configurations
{
    public class UICSVConfiguration : UICSVConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_uicsv_configuration";

        protected override void ConfigureDatabase(EntityTypeBuilder<Import.CSV.UICSVConfiguration> builder)
        {
            builder.ToTable("uicsv_configuration", Options.Scheme);

            builder.Property(x => x.ID)
                .HasColumnName("id");
            builder.Property(x => x.Name)
                .HasColumnName("name");
            builder.Property(x => x.Note)
                .HasColumnName("note");
            builder.Property(x => x.Delimiter)
                .HasColumnName("delimiter");

            builder.HasXminRowVersion(e => e.RowVersion);
        }
    }
}