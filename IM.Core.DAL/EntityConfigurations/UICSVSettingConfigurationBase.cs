using InfraManager.DAL.Import.CSV;
using InfraManager.DAL.Import;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class UICSVSettingConfigurationBase : IEntityTypeConfiguration<UICSVSetting>
    {
        public void Configure(EntityTypeBuilder<UICSVSetting> builder)
        {
            builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

            builder.Property(x => x.ID)
                .IsRequired(true);
            builder.Property(x => x.CSVConfigurationID);
            builder.Property(x => x.Path)
               .HasMaxLength(500)
               .IsRequired(true);

            builder.HasOne(x => x.Configuration)
                .WithOne()
                .HasForeignKey<UICSVSetting>(d => d.CSVConfigurationID)
                .HasConstraintName(ForeignKeyName);

            ConfigureDatabase(builder);

        }
        protected abstract void ConfigureDatabase(EntityTypeBuilder<UICSVSetting> builder);

        protected abstract string PrimaryKeyName { get; }
        protected abstract string ForeignKeyName { get; }
    }
}