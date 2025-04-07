using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.Software;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class SoftwareInstallationConfiguration : SoftwareInstallationConfigurationBase
{
    protected override string PrimaryKeyName => "pk_software_installation";
    protected override string SoftwareModelForeignKey => "fk_software_installation_software_model";
    protected override string SoftwareLicenceForeignKey => "fk_software_installation_software_licence";
    protected override string IndexByDeviceID => "ix_software_installation_device_id";
    protected override string IndexBySoftwareModelID => "ix_software_installation_software_model_id";
    protected override string IndexBySoftwareLicenceID => "ix_software_installation_software_licence_id";
    protected override string IndexBySoftwareLicenceSerialNumberID => "ix_software_installation_software_licence_serial_number_id";

    public override void ConfigureDataBase(EntityTypeBuilder<SoftwareInstallation> builder)
    {
        builder.ToTable("software_installation", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("id");
        builder.Property(e => e.State).HasColumnName("state");
        builder.Property(e => e.DeviceID).HasColumnName("device_id");
        builder.Property(e => e.RegistryID).HasColumnName("registry_id");
        builder.Property(e => e.InstallPath).HasColumnName("install_path");
        builder.Property(e => e.UniqueNumber).HasColumnName("unique_number");
        builder.Property(e => e.DeviceClassID).HasColumnName("device_class_id");
        builder.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(e => e.SoftwareModelID).HasColumnName("software_model_id");
        builder.Property(e => e.SoftwareLicenceID).HasColumnName("software_licence_id");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
        builder.Property(e => e.SoftwareExecutionCount).HasColumnName("software_execution_count");
        builder.Property(e => e.SoftwareLicenceSerialNumberID).HasColumnName("software_licence_serial_number_id");

        builder.Property(e => e.InstallDate).HasColumnName("install_date")
            .HasColumnType("timestamp without time zone");
        builder.Property(e => e.UtcDateCreated).HasColumnName("utc_date_created")
            .HasColumnType("timestamp without time zone");
        builder.Property(e => e.UtcDateLastDetected).HasColumnName("utc_date_last_detected")
            .HasColumnType("timestamp without time zone");

        builder.HasXminRowVersion(e => e.RowVersion);
    }
}