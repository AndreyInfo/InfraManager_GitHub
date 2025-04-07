using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Software.Installation;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    public partial class ViewSoftwareInstallationConfiguration : IEntityTypeConfiguration<ViewSoftwareInstallation>
    {
        public void Configure(EntityTypeBuilder<ViewSoftwareInstallation> entity)
        {
            entity.HasNoKey();

            entity.ToView("view_SoftwareInstallation", "dbo");

            entity.Property(e => e.CommercialManufacturerName).HasMaxLength(255);

            entity.Property(e => e.CommercialModelId).HasColumnName("CommercialModelID");

            entity.Property(e => e.CommercialModelName).HasMaxLength(250);

            entity.Property(e => e.CommercialModelUsingTypeId).HasColumnName("CommercialModelUsingTypeID");

            entity.Property(e => e.CommercialModelUsingTypeName).HasMaxLength(250);

            entity.Property(e => e.CommercialModelVersion).HasMaxLength(50);

            entity.Property(e => e.CommercialTypeId).HasColumnName("CommercialTypeID");

            entity.Property(e => e.CommercialTypeName).HasMaxLength(250);

            entity.Property(e => e.DeviceClassId).HasColumnName("DeviceClassID");

            entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

            entity.Property(e => e.DeviceName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.DeviceOrganizationName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.DeviceOwnerName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.DeviceUtilizerName)
                .IsRequired()
                .HasMaxLength(2258);

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.Property(e => e.InstallDate).HasColumnType("datetime");

            entity.Property(e => e.InstallPath).HasMaxLength(1000);

            entity.Property(e => e.ManufacturerName).HasMaxLength(255);

            entity.Property(e => e.RowVersion)
                .IsRequired()
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.Property(e => e.SerialNumber).HasMaxLength(500);

            entity.Property(e => e.SoftwareLicenceId).HasColumnName("SoftwareLicenceID");

            entity.Property(e => e.SoftwareLicenceName).HasMaxLength(250);

            entity.Property(e => e.SoftwareLicenceSchemeName).HasMaxLength(250);

            entity.Property(e => e.SoftwareLicenceSerialNumberId).HasColumnName("SoftwareLicenceSerialNumberID");

            entity.Property(e => e.SoftwareModelId).HasColumnName("SoftwareModelID");

            entity.Property(e => e.SoftwareModelName)
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(e => e.SoftwareModelUsingTypeId).HasColumnName("SoftwareModelUsingTypeID");

            entity.Property(e => e.SoftwareModelUsingTypeName)
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(e => e.SoftwareModelVersion)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.SoftwareTypeId).HasColumnName("SoftwareTypeID");

            entity.Property(e => e.SoftwareTypeName)
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(e => e.UniqueNumber)
                .IsRequired()
                .HasMaxLength(500);

            //entity.Property(e => e.UtcDateCreated).HasColumnType("datetime");
            entity.Ignore(e => e.UtcDateCreated);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<ViewSoftwareInstallation> entity);
    }
}
