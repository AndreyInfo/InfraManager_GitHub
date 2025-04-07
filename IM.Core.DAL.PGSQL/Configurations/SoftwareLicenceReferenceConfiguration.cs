using InfraManager.DAL.Software;
using IM.Core.DAL.PGSQL;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using IM.Core.DAL.Postgres;

namespace IM.Core.DAL.PGSQL.Configurations
{
    public partial class SoftwareLicenceReferenceConfiguration : IEntityTypeConfiguration<SoftwareLicenceReference>
    {
        public void Configure(EntityTypeBuilder<SoftwareLicenceReference> entity)
        {
            entity.ToTable("software_licence_reference", Options.Scheme);

            entity.HasKey(e => new { e.ObjectId, e.ClassId, e.SoftwareLicenceId })
                .HasName("pk__software_licence_reference");

            //entity.HasIndex(e => new { e.SoftwareLicenceId, e.ObjectId, e.SoftwareSubLicenceId }, "fk_software_licence_reference")
            //    .IsUnique();

            //entity.HasIndex(e => e.ObjectId, "ix_software_licence_reference_object_id");

            //entity.HasIndex(e => e.SoftwareLicenceId, "ix_software_licence_reference_software_licence_id");

            //entity.HasIndex(e => e.SoftwareLicenceSerialNumberId, "ix_software_licence_reference_software_licence_serial_number_id");

            entity.Property(e => e.ObjectId)
                .HasColumnName("object_id")
                .HasComment("Ссылка лицензии на внешний объект (оборудование, либо пользователя)");

            entity.Property(e => e.ClassId)
                .HasColumnName("class_id")
                .HasComment("Класс объекта");

            entity.Property(e => e.SoftwareLicenceId)
                .HasColumnName("software_licence_id")
                .HasComment("Идентификатор лицензии ПО");
            entity.Property(e => e.SoftwareExecutionCount).HasColumnName("software_execution_count");
            entity.Property(e => e.UniqueNumber).HasColumnName("unique_number");


            entity.Property(e => e.PeripheralDatabaseId).HasColumnName("peripheral_database_id");

            entity.Property(e => e.SoftwareLicenceSerialNumberId).HasColumnName("software_licence_serial_number_id");

            entity.Property(e => e.SoftwareSubLicenceId).HasColumnName("software_sub_licence_id");

            entity.Property(e => e.UniqueNumber)
                .IsRequired()
                .HasMaxLength(500);

            entity.HasOne(d => d.SoftwareLicence)
                .WithMany(p => p.SoftwareLicenceReference)
                .HasForeignKey(d => d.SoftwareLicenceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_software_licence_reference_software_licence");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<SoftwareLicenceReference> entity);
    }
}
