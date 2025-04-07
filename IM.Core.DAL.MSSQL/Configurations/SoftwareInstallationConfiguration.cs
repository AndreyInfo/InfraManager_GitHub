using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Software;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class SoftwareInstallationConfiguration : SoftwareInstallationConfigurationBase
{
    protected override string PrimaryKeyName => "PK_SoftwareInstallation";
    protected override string SoftwareModelForeignKey => "FK_SoftwareInstallation_SoftwareModel";
    protected override string SoftwareLicenceForeignKey => "FK_SoftwareInstallation_SoftwareLicence";
    protected override string IndexByDeviceID => "IX_SoftwareInstallation_DeviceID";
    protected override string IndexBySoftwareModelID => "IX_SoftwareInstallation_SoftwareModelID";
    protected override string IndexBySoftwareLicenceID => "IX_SoftwareInstallation_SoftwareLicenceID";
    protected override string IndexBySoftwareLicenceSerialNumberID => "IX_SoftwareInstallation_SoftwareLicenceSerialNumberID";

    public override void ConfigureDataBase(EntityTypeBuilder<SoftwareInstallation> builder)
    {
        builder.ToTable("SoftwareInstallation", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("ID");
        builder.Property(e => e.State).HasColumnName("State");
        builder.Property(e => e.DeviceID).HasColumnName("DeviceID");
        builder.Property(e => e.RegistryID).HasColumnName("RegistryID");
        builder.Property(e => e.InstallPath).HasColumnType("InstallPath");
        builder.Property(e => e.UniqueNumber).HasColumnName("UniqueNumber");
        builder.Property(e => e.DeviceClassID).HasColumnName("DeviceClassID");
        builder.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(e => e.SoftwareModelID).HasColumnName("SoftwareModelID");
        builder.Property(e => e.SoftwareLicenceID).HasColumnName("SoftwareLicenceID");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
        builder.Property(e => e.SoftwareExecutionCount).HasColumnName("SoftwareExecutionCount");
        builder.Property(e => e.SoftwareLicenceSerialNumberID).HasColumnName("SoftwareLicenceSerialNumberID");

        builder.Property(e => e.InstallDate).HasColumnType("datetime").HasColumnName("InstallDate");
        builder.Property(e => e.UtcDateCreated).HasColumnType("datetime").HasColumnName("UtcDateCreated");
        builder.Property(e => e.UtcDateLastDetected).HasColumnType("datetime").HasColumnName("UtcDateLastDetected");
        
        builder.Property(e => e.RowVersion)
            .IsRowVersion()
            .HasColumnType("timestamp")
            .HasColumnName("RowVersion");
    }
}
