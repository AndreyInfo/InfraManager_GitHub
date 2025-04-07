using InfraManager.DAL.Software;
using IM.Core.DAL.PGSQL;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using IM.Core.DAL.Postgres;


namespace IM.Core.DAL.PGSQL.Configurations
{
    public partial class SoftwareLicenceSchemeProcessorCoeffConfiguration : IEntityTypeConfiguration<SoftwareLicenceSchemeProcessorCoeff>
    {
        public void Configure(EntityTypeBuilder<SoftwareLicenceSchemeProcessorCoeff> entity)
        {
            entity.ToTable("software_licence_scheme_processor_coeff", Options.Scheme);
            entity.HasKey(e => new { e.LicenceSchemeId, e.ProcessorTypeId })
                .HasName("pk_software_licence_scheme_processor_coeff");

            entity.Property(e => e.LicenceSchemeId).HasColumnName("licence_scheme_id");

            entity.Property(e => e.ProcessorTypeId).HasColumnName("processor_type_id");
            entity.Property(e => e.Coefficient).HasColumnName("coefficient");

            entity.HasOne(d => d.SoftwareLicenceScheme)
                .WithMany(p => p.SoftwareLicenceSchemeCoefficients)
                .HasForeignKey(d => d.LicenceSchemeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_software_licence_serial_number_software_licence");
            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<SoftwareLicenceSchemeProcessorCoeff> entity);
    }
}
