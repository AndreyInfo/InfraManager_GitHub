using InfraManager.DAL.Software;
using IM.Core.DAL.PGSQL;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    public partial class SoftwareLicenceSchemeConfiguration : IEntityTypeConfiguration<SoftwareLicenceScheme>
    {
        public void Configure(EntityTypeBuilder<SoftwareLicenceScheme> entity)
        {
            entity.ToTable("software_licence_scheme", Options.Scheme);

            entity.HasKey(e => e.ID)
                .HasName("pk_software_licence_scheme");

            entity.Property(e => e.ID)
                .ValueGeneratedNever()
                .HasColumnName("id");

            entity.Property(e => e.AdditionalRights).HasMaxLength(4000);


            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.Property(e => e.Description).HasMaxLength(4000);


            entity.Property(e => e.LicenseCountPerObject).HasMaxLength(4000);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.Property(e => e.AdditionalRights).HasColumnName("additional_rights");
            entity.Property(e => e.CompatibilityTypeID).HasColumnName("compatibility_type_id");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ID).HasColumnName("id");
            entity.Property(e => e.IncreaseForVm).HasColumnName("increase_for_vm");
            entity.Property(e => e.InstallationLimitPerVm).HasColumnName("installation_limit_per_vm");
            entity.Property(e => e.InstallationLimits).HasColumnName("installation_limits");
            entity.Property(e => e.IsAllowInstallOnVm).HasColumnName("is_allow_install_on_vm");
            entity.Property(e => e.IsCanHaveSubLicence).HasColumnName("is_can_have_sub_licence");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.IsLicenceByAccess).HasColumnName("is_licence_by_access");
            entity.Property(e => e.IsLicenseAllHosts).HasColumnName("is_license_all_hosts");
            entity.Property(e => e.IsLinkLicenseToObject).HasColumnName("is_link_license_to_object");
            entity.Property(e => e.IsLinkLicenseToUser).HasColumnName("is_link_license_to_user");
            entity.Property(e => e.LicenseCountPerObject).HasColumnName("license_count_per_object");
            entity.Property(e => e.LicensingObjectType).HasColumnName("licensing_object_type");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.SchemeType).HasColumnName("scheme_type");
            entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");


            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<SoftwareLicenceScheme> entity);
    }
}
