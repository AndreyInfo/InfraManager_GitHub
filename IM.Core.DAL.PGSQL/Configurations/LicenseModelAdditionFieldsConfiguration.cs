using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Software;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;

public class LicenseModelAdditionFieldsConfiguration : LicenseModelAdditionFieldsConfigurationBase
{
    protected override string KeyName => "pk_license_model_control";

    protected override string ForeignKeyLicenseModelAdditionFields => "fk_control_software_model";

    protected override void ConfigureDataBase(EntityTypeBuilder<LicenseModelAdditionFields> builder)
    {
        builder.ToTable("license_model_addition_fields", Options.Scheme);

        builder.Property(e => e.SoftwareModelID).HasColumnName("software_model_id");
        builder.Property(e => e.LicenseControlID).HasColumnName("license_control_id");
        builder.Property(e => e.LanguageID).HasColumnName("language_id");
    }
}
