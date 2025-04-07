using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Asset;

namespace IM.Core.DAL.PGSQL.Configurations
{
    public partial class ElpsettingConfiguration : IEntityTypeConfiguration<ElpSetting>
    {
        public void Configure(EntityTypeBuilder<ElpSetting> entity)
        {
            entity.ToTable("elp_setting", Options.Scheme);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.VendorId).HasColumnName("vendor_id");

            entity.HasKey(e => e.Id).HasName("pk_elp_setting");
            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(500);

            entity.Property(e => e.Note).HasColumnName("note").HasMaxLength(1000);

            entity.Property(e => e.VendorId).HasColumnName("vendor_id");

            entity.HasOne(d => d.Vendor)
                .WithMany(p => p.Elpsetting)
                .HasPrincipalKey(p => p.ImObjID)
                .HasForeignKey(d => d.VendorId)
                .HasConstraintName("fk_elp_setting__vendor_id");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<ElpSetting> entity);
    }
}
