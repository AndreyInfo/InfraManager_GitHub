using InfraManager.DAL.Import.CSV;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class UICSVConfigurationBase : IEntityTypeConfiguration<UICSVConfiguration>
    {
        public void Configure(EntityTypeBuilder<UICSVConfiguration> builder)
        {
            builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

            builder.Property(x => x.ID)
                .IsRequired(true);
            builder.Property(x => x.Name)
                .IsRequired(true)   
                .HasMaxLength(250);
            builder.Property(x => x.Note)
                .IsRequired(true)
                .HasMaxLength(500);
            builder.Property(x => x.Delimiter)
                .IsRequired(true)
                .HasMaxLength(1);

            builder.Property(x => x.RowVersion)
                .IsRowVersion()
                .IsRequired(true)
                .HasColumnType("timestamp");


            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<UICSVConfiguration> builder);

        protected abstract string PrimaryKeyName { get; }

    }
}
