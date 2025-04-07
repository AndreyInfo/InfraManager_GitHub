using InfraManager.DAL.Software;
using IM.Core.DAL.PGSQL;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using IM.Core.DAL.Postgres;


namespace IM.Core.DAL.PGSQL.Configurations
{
    public partial class SoftwareInstallationDependancesConfiguration : IEntityTypeConfiguration<SoftwareInstallationDependances>
    {
        public void Configure(EntityTypeBuilder<SoftwareInstallationDependances> entity)
        {
            entity.ToTable("software_installation_dependances", Options.Scheme);

            entity.HasKey(e => new { e.InstallationId, e.DependantInstallationId })
                .HasName("pk_software_installation_dependances");

            entity.Property(e => e.InstallationId).HasColumnName("installation_id");

            entity.Property(e => e.DependantInstallationId).HasColumnName("dependant_installation_id");

            entity.HasOne(d => d.DependantInstallation)
                .WithMany(p => p.SoftwareInstallationDependancesDependantInstallation)
                .HasForeignKey(d => d.DependantInstallationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_software_installation_dependance_dependant");

            entity.HasOne(d => d.Installation)
                .WithMany(p => p.SoftwareInstallationDependancesInstallation)
                .HasForeignKey(d => d.InstallationId)
                .HasConstraintName("fk_software_installation_dependance_primary");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<SoftwareInstallationDependances> entity);
    }
}
