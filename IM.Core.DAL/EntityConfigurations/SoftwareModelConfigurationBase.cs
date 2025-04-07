using InfraManager.DAL.Software;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class SoftwareModelConfigurationBase : IEntityTypeConfiguration<SoftwareModel>
    {
        protected abstract string KeyName { get; }

        public void Configure(EntityTypeBuilder<SoftwareModel> builder)
        {
            builder.HasKey(e => e.ID).HasName(KeyName);

            builder.IsMarkableForDelete();

            builder.Property(e => e.ID);

            builder.Property(e => e.SoftwareTypeID)
                .IsRequired(true);

            builder.Property(e => e.Name)
                .IsRequired(true)
                .HasMaxLength(250);

            builder.Property(e => e.Note)
                .IsRequired(true)
                .HasMaxLength(500);

            builder.Property(e => e.Version)
                .IsRequired(true)
                .HasMaxLength(50);

            builder.Property(e => e.Code)
                .IsRequired(true)
                .HasMaxLength(50);

            builder.Property(e => e.ManufacturerID)
                .IsRequired(false);

            builder.Property(e => e.SupportDate)
                .IsRequired(false);

            builder.Property(e => e.Template)
                .IsRequired(true);

            builder.Property(e => e.Removed)
                .IsRequired(true);

            builder.Property(e => e.ParentID)
                .IsRequired(false);

            builder.Property(e => e.TrueID)
                .IsRequired(false);

            builder.Property(e => e.CreateDate)
                .IsRequired(true);

            builder.Property(e => e.SoftwareModelUsingTypeID)
                .IsRequired(true);

            builder.Property(e => e.IsCommercial)
                .IsRequired(true);

            builder.Property(e => e.CommercialModelID)
                .IsRequired(false);

            builder.Property(e => e.ProcessNames)
                .IsRequired(true);

            builder.Property(e => e.ExternalID)
                .IsRequired(true)
                .HasMaxLength(250);

            builder.Property(e => e.UtcDateCreated)
                .IsRequired(true);

            builder.Property(e => e.ModelRedaction)
                .IsRequired(false)
                .HasMaxLength(250);

            builder.Property(e => e.OwnerModelID)
                .IsRequired(false);

            builder.Property(e => e.OwnerModelClassID)
                .IsRequired(false);

            builder.Property(e => e.ModelDistribution)
                .IsRequired(false)
                .HasMaxLength(250);

            builder.Property(e => e.PercentComponent)
                .IsRequired(false);

            builder.Property(e => e.ComplementaryID)
                .IsRequired(false);



            builder.HasOne(d => d.CommercialModel)
                .WithMany(p => p.InverseCommercialModel)
                .HasForeignKey(d => d.CommercialModelID);

            builder.HasOne(d => d.Parent)
                .WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentID);

            builder.HasOne(d => d.SoftwareModelUsingType)
                .WithMany(p => p.SoftwareModel)
                .HasForeignKey(d => d.SoftwareModelUsingTypeID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.SoftwareType)
                .WithMany(p => p.SoftwareModel)
                .HasForeignKey(d => d.SoftwareTypeID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.True)
                .WithMany(p => p.InverseTrue)
                .HasForeignKey(d => d.TrueID);

            ConfigureDataBase(builder);
        }
        protected abstract void ConfigureDataBase(EntityTypeBuilder<SoftwareModel> builder);

    }
}
