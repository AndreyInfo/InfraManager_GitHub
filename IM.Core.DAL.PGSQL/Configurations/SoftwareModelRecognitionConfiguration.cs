using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Software;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;

public class SoftwareModelRecognitionConfiguration : SoftwareModelRecognitionConfigurationBase
{
    protected override string KeyName => "pk___software__1_f0_a52_e28_c2_e296_c";

    protected override string ForeignKeySoftwareModelRecognition => "fk_software_model_software_model_recognition";

    protected override void ConfigureDataBase(EntityTypeBuilder<SoftwareModelRecognition> builder)
    {
        builder.ToTable("software_model_recognition", Options.Scheme);

        builder.Property(e => e.SoftwareModelID).HasColumnName("software_model_id");
        builder.Property(e => e.VersionRecognitionID).HasColumnName("version_recognition_id");
        builder.Property(e => e.VersionRecognitionLvl).HasColumnName("version_recognition_lvl");
        builder.Property(e => e.RedactionRecognition).HasColumnName("redaction_recognition");
    }
}
