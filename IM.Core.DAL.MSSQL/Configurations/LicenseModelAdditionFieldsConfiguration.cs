using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Software;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;

public class LicenseModelAdditionFieldsConfiguration : LicenseModelAdditionFieldsConfigurationBase
{
    protected override string KeyName => "PK_LicenseModelControl";

    protected override string ForeignKeyLicenseModelAdditionFields => "FK_Control_SoftwareModel";

    protected override void ConfigureDataBase(EntityTypeBuilder<LicenseModelAdditionFields> builder)
    {
        builder.ToTable("LicenseModelAdditionFields", "dbo");

        builder.Property(e => e.SoftwareModelID).HasColumnName("SoftwareModelID");
        builder.Property(e => e.LicenseControlID).HasColumnName("LicenseControlID");
        builder.Property(e => e.LanguageID).HasColumnName("LanguageID");
    }
}
