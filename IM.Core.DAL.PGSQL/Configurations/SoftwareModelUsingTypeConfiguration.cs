using IM.Core.DAL.Postgres;
using InfraManager.DAL.Software;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Postgres;

namespace IM.Core.DAL.PGSQL.Configurations
{
    public partial class SoftwareModelUsingTypeConfiguration : IEntityTypeConfiguration<SoftwareModelUsingType>
    {
        public void Configure(EntityTypeBuilder<SoftwareModelUsingType> entity)
        {
            entity.ToTable("software_model_using_type", Options.Scheme);
            entity.Property(e => e.IsDefault).HasColumnName("is_default");

            entity.HasKey(e => e.Id).HasName("pk_software_model_using_type");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");

            entity.Property(e => e.Name).HasColumnName("name")
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(e => e.Note).HasColumnName("note")
                .IsRequired()
                .HasMaxLength(500);

            entity.HasXminRowVersion(e => e.RowVersion);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<SoftwareModelUsingType> entity);
    }
}