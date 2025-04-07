using InfraManager.DAL.Software;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class LicenseModelAdditionFieldsConfigurationBase : IEntityTypeConfiguration<LicenseModelAdditionFields>
{
    protected abstract string KeyName { get; }
    protected abstract string ForeignKeyLicenseModelAdditionFields { get; }

    public void Configure(EntityTypeBuilder<LicenseModelAdditionFields> builder)
    {
        builder.HasKey(e => e.SoftwareModelID).HasName(KeyName);

        builder.Property(e => e.SoftwareModelID).IsRequired(true);
        builder.Property(e => e.LicenseControlID).IsRequired(false);
        builder.Property(e => e.LanguageID).IsRequired(false);

        builder.HasOne(d => d.SoftwareModel)
            .WithOne(p => p.LicenseModelAdditionFields)
            .HasForeignKey<LicenseModelAdditionFields>(d => d.SoftwareModelID)
            .HasConstraintName(ForeignKeyLicenseModelAdditionFields);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<LicenseModelAdditionFields> builder);
}
