using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Software;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;

public class SoftwareModelRecognitionConfiguration : SoftwareModelRecognitionConfigurationBase
{
    protected override string KeyName => "PK__Software__1F0A52E28C2E296C";

    protected override string ForeignKeySoftwareModelRecognition => "FK_SoftwareModel_SoftwareModelRecognition";

    protected override void ConfigureDataBase(EntityTypeBuilder<SoftwareModelRecognition> builder)
    {
        builder.ToTable("SoftwareModelRecognition", "dbo");

        builder.Property(e => e.SoftwareModelID).HasColumnName("SoftwareModelID");
        builder.Property(e => e.VersionRecognitionID).HasColumnName("VersionRecognitionID");
        builder.Property(e => e.VersionRecognitionLvl).HasColumnName("VersionRecognitionLvl");
        builder.Property(e => e.RedactionRecognition).HasColumnName("RedactionRecognition");
    }
}
