using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Software.Installation;
using InfraManager.DAL.Postgres;

namespace IM.Core.DAL.PGSQL.Configurations
{
    public partial class ViewSoftwareInstallationConfiguration : IEntityTypeConfiguration<ViewSoftwareInstallation>
    {
        public void Configure(EntityTypeBuilder<ViewSoftwareInstallation> entity)
        {
            entity.HasNoKey();

            entity.ToView("view_software_installation", Options.Scheme);

            entity.Property(e => e.CommercialManufacturerName).HasColumnName("commercial_manufacturer_name")
                .HasMaxLength(255);

            entity.Property(e => e.CommercialModelId).HasColumnName("commercial_model_id");

            entity.Property(e => e.CommercialModelName).HasColumnName("commercial_model_name").HasMaxLength(250);

            entity.Property(e => e.CommercialModelUsingTypeId).HasColumnName("commercial_model_using_type_id");

            entity.Property(e => e.CommercialModelUsingTypeName).HasColumnName("commercial_model_using_type_name")
                .HasMaxLength(250);

            entity.Property(e => e.CommercialModelVersion).HasColumnName("commercial_model_version").HasMaxLength(50);

            entity.Property(e => e.CommercialTypeId).HasColumnName("commercial_type_id");

            entity.Property(e => e.CommercialTypeName).HasColumnName("commercial_type_name").HasMaxLength(250);

            entity.Property(e => e.DeviceClassId).HasColumnName("device_class_id");

            entity.Property(e => e.DeviceId).HasColumnName("device_id");

            entity.Property(e => e.DeviceName).HasColumnName("device_name")
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.DeviceOrganizationName).HasColumnName("device_organization_name")
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.DeviceOwnerName).HasColumnName("device_owner_name")
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.DeviceUtilizerName).HasColumnName("device_utilizer_name")
                .IsRequired()
                .HasMaxLength(2258);

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.InstallDate).HasColumnName("install_date").HasColumnType("datetime");

            entity.Property(e => e.InstallPath).HasColumnName("install_path").HasMaxLength(1000);

            entity.Property(e => e.ManufacturerName).HasColumnName("manufacturer_name").HasMaxLength(255);

            entity.HasXminRowVersion(e => e.RowVersion);

            entity.Property(e => e.SerialNumber).HasColumnName("serial_number").HasMaxLength(500);

            entity.Property(e => e.SoftwareLicenceId).HasColumnName("software_licence_id");

            entity.Property(e => e.SoftwareLicenceName).HasColumnName("software_licence_name").HasMaxLength(250);

            entity.Property(e => e.SoftwareLicenceScheme).HasColumnName("software_licence_scheme");

            entity.Property(e => e.SoftwareLicenceSchemeName).HasColumnName("software_licence_scheme_name")
                .HasMaxLength(250);

            entity.Property(e => e.SoftwareLicenceSerialNumberId).HasColumnName("software_licence_serial_number_id");

            entity.Property(e => e.SoftwareModelId).HasColumnName("software_model_id");

            entity.Property(e => e.SoftwareModelName).HasColumnName("software_model_name")
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(e => e.SoftwareModelUsingTypeId).HasColumnName("software_model_using_type_id");

            entity.Property(e => e.SoftwareModelUsingTypeName).HasColumnName("software_model_using_type_name")
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(e => e.SoftwareModelVersion).HasColumnName("software_model_version")
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.SoftwareTypeId).HasColumnName("software_type_id");

            entity.Property(e => e.SoftwareTypeName).HasColumnName("software_type_name")
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(e => e.UniqueNumber).HasColumnName("unique_number")
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.SoftwareExecutionCount).HasColumnName("software_execution_count");

            entity.Property(e => e.State).HasColumnName("state");

            entity.Property(e => e.UtcDateLastDetected).HasColumnName("utc_date_last_detected");

            //entity.Property(e => e.UtcDateCreated).HasColumnType("datetime");
            entity.Ignore(e => e.UtcDateCreated);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<ViewSoftwareInstallation> entity);
    }
}