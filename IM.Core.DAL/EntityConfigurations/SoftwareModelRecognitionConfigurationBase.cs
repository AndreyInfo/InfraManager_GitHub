using InfraManager.DAL.Software;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class SoftwareModelRecognitionConfigurationBase : IEntityTypeConfiguration<SoftwareModelRecognition>
{
    protected abstract string KeyName { get; }
    protected abstract string ForeignKeySoftwareModelRecognition { get; }

    public void Configure(EntityTypeBuilder<SoftwareModelRecognition> builder)
    {
        builder.HasKey(e => e.SoftwareModelID).HasName(KeyName);

        builder.Property(e => e.SoftwareModelID).IsRequired(true);
        builder.Property(e => e.VersionRecognitionID).IsRequired(true);
        builder.Property(e => e.VersionRecognitionLvl).IsRequired(true);
        builder.Property(e => e.RedactionRecognition).IsRequired(true);

        builder.HasOne(d => d.SoftwareModel)
            .WithOne(p => p.SoftwareModelRecognition)
            .HasForeignKey<SoftwareModelRecognition>(d => d.SoftwareModelID)
            .HasConstraintName(ForeignKeySoftwareModelRecognition);

        ConfigureDataBase(builder);
    }
    protected abstract void ConfigureDataBase(EntityTypeBuilder<SoftwareModelRecognition> builder);

}
