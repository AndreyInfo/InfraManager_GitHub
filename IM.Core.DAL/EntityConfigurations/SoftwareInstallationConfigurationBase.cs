using InfraManager.DAL.Software;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class SoftwareInstallationConfigurationBase : IEntityTypeConfiguration<SoftwareInstallation>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string SoftwareModelForeignKey { get; }
    protected abstract string SoftwareLicenceForeignKey { get; }
    protected abstract string IndexByDeviceID { get; }
    protected abstract string IndexBySoftwareModelID { get; }
    protected abstract string IndexBySoftwareLicenceID { get; }
    protected abstract string IndexBySoftwareLicenceSerialNumberID { get; }

    public void Configure(EntityTypeBuilder<SoftwareInstallation> builder)
    {
        builder.HasKey(c => c.ID).HasName(PrimaryKeyName);

        builder.HasIndex(e => e.DeviceID, IndexByDeviceID);
        builder.HasIndex(e => e.SoftwareModelID, IndexBySoftwareModelID);
        builder.HasIndex(e => e.SoftwareLicenceID, IndexBySoftwareLicenceID);
        builder.HasIndex(e => e.SoftwareLicenceSerialNumberID, IndexBySoftwareLicenceSerialNumberID);


        builder.Property(e => e.ID)
            .ValueGeneratedNever()
            .HasComment("Идентификатор инсталляции ПО");

        builder.Property(e => e.DeviceClassID).HasComment("Класс устройства, на котором установлена инсталляция");
        builder.Property(e => e.DeviceID).HasComment("Ссылка на устройство, на котором установлена инсталляция");
        builder.Property(e => e.InstallDate).HasComment("Дата установки ПО");
        builder.Property(e => e.SoftwareLicenceID).HasComment("Ссылка на лицензию ПО");
        builder.Property(e => e.SoftwareLicenceSerialNumberID).HasComment("Ссылка на серийный номер");
        builder.Property(e => e.SoftwareModelID).HasComment("Ссылка на модель ПО");

        builder.Property(e => e.InstallPath)
            .IsRequired(false)
            .HasMaxLength(1000)
            .HasComment("Путь установки ПО");
        
        builder.Property(e => e.RegistryID)
            .IsRequired(true)
            .HasMaxLength(100);

        builder.Property(e => e.UniqueNumber)
            .IsRequired(true)
            .HasMaxLength(500)
            .HasComment("Регистрационный ключ (или серийный номер) инсталляции");


        builder.HasOne(d => d.SoftwareLicence)
            .WithMany(p => p.SoftwareInstallation)
            .HasForeignKey(d => d.SoftwareLicenceID)
            .HasConstraintName(SoftwareLicenceForeignKey);

        builder.HasOne(d => d.SoftwareModel)
            .WithMany(p => p.SoftwareInstallation)
            .HasForeignKey(d => d.SoftwareModelID)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName(SoftwareModelForeignKey);

        ConfigureDataBase(builder);
    }

    public abstract void ConfigureDataBase(EntityTypeBuilder<SoftwareInstallation> builder);
}
