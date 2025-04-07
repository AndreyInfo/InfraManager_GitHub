using IM.Core.DAL.Postgres;
using InfraManager.DAL.Software;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Postgres;

namespace IM.Core.DAL.PGSQL.Configurations
{
    public partial class SoftwareTypeConfiguration : IEntityTypeConfiguration<SoftwareType>
    {
        public void Configure(EntityTypeBuilder<SoftwareType> entity)
        {
            entity.ToTable("software_type", Options.Scheme);

            entity.HasKey(e => e.ID).HasName("pk_software_type");


            //entity.HasIndex(e => e.ParentId, "ix_software_type_parent_id");

            entity.Property(e => e.ID)
                .ValueGeneratedNever()
                .HasColumnName("id")
                .HasComment("Идентификатор типа ПО");

            entity.Property(e => e.ComplementaryId).HasColumnName("complementary_id");

            entity.Property(e => e.Name).HasColumnName("name")
                .IsRequired()
                .HasMaxLength(250)
                .HasComment("Название типа ПО");

            entity.Property(e => e.Note).HasColumnName("note")
                .IsRequired()
                .HasMaxLength(500)
                .HasComment("Описание типа ПО");

            entity.Property(e => e.ParentId)
                .HasColumnName("parent_id")
                .HasComment("Ссылка на родительский тип ПО");

            entity.HasXminRowVersion(e => e.RowVersion);

            entity.HasOne(d => d.Parent)
                .WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("fk_software_type_software_type");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<SoftwareType> entity);
    }
}