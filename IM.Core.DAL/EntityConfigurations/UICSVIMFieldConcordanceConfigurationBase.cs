using InfraManager.DAL.Import.CSV;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class UICSVIMFieldConcordanceConfigurationBase : IEntityTypeConfiguration<UICSVIMFieldConcordance>
    {
        public void Configure(EntityTypeBuilder<UICSVIMFieldConcordance> builder)
        {
            builder.HasKey(x => new { x.CSVConfigurationID, x.IMFieldID }).HasName(PrimaryKeyName);

            builder.Property(x => x.CSVConfigurationID)
                .IsRequired(true);
            builder.Property(x => x.IMFieldID)
                .IsRequired(true);
            builder.Property(x => x.Expression)
                .IsRequired(true)
                .HasMaxLength(1024);

            builder.HasOne(x => x.Configuration)
                .WithMany()
                .HasForeignKey(e => e.CSVConfigurationID)
                .HasConstraintName(ForeignKeyName);

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<UICSVIMFieldConcordance> builder);

        protected abstract string PrimaryKeyName { get; }

        protected abstract string ForeignKeyName { get; }

    }
}
