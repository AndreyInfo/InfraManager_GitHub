using Inframanager.DAL.ActiveDirectory.Import;
using InfraManager.DAL.Import;
using InfraManager.DAL.Import.CSV;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class UISettingConfigurationBase : IEntityTypeConfiguration<UISetting>
    {
        public void Configure(EntityTypeBuilder<UISetting> builder)
        {
            builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

            builder.HasQueryFilter(x => !x.Removed);

            builder.Property(x => x.ID).IsRequired(true);
            builder.Property(x => x.Name).IsRequired(true).HasMaxLength(250);
            builder.Property(x => x.Note).IsRequired().HasMaxLength(500);
            builder.Property(x => x.ProviderType).IsRequired();
            builder.Property(x => x.ObjectType).IsRequired();
            builder.Property(x => x.LocationMode).IsRequired();
            builder.Property(x => x.RestoreRemovedUsers).IsRequired();
            builder.Property(x => x.UpdateLocation).IsRequired();
            builder.Property(x => x.UpdateSubdivision).IsRequired();
            builder.Property(x => x.OrganizationComparison).IsRequired();
            builder.Property(x => x.SubdivisionComparison).IsRequired(true);
            builder.Property(x => x.UserComparison).IsRequired(true);
            builder.Property(x => x.LocationItemID);
            builder.Property(x => x.SubdivisionDefaultOrganizationItemClassID);
            builder.Property(x => x.SubdivisionDefaultOrganizationItemID);
            builder.Property(x => x.UserDefaultOrganizationItemID);
            builder.Property(x => x.UserAddedWorkflowEnabled).IsRequired(true);
            builder.Property(x => x.UserAddedWorkflowSchemeIdentifier).HasMaxLength(100);
            builder.Property(x => x.UserModifiedWorkflowEnabled).IsRequired(true);
            builder.Property(x => x.UserModifiedWorkflowSchemeIdentifier).HasMaxLength(100);
            builder.Property(x => x.UserRemovedWorkflowEnabled).IsRequired(true);
            builder.Property(x => x.UserRemovedWorkflowSchemeIdentifier).HasMaxLength(100);
            builder.Property(x => x.DivisionAddedWorkflowEnabled).IsRequired(true);
            builder.Property(x => x.DivisionAddedWorkflowSchemeIdentifier).HasMaxLength(100);
            builder.Property(x => x.DivisionModifiedWorkflowEnabled).IsRequired(true);
            builder.Property(x => x.DivisionModifiedWorkflowSchemeIdentifier).HasMaxLength(100);
            builder.Property(x => x.OrganizationAddedWorkflowEnabled).IsRequired(true);
            builder.Property(x => x.OrganizationAddedWorkflowSchemeIdentifier).HasMaxLength(100);
            builder.Property(x => x.OrganizationModifiedWorkflowEnabled).IsRequired(true);
            builder.Property(x => x.OrganizationModifiedWorkflowSchemeIdentifier).HasMaxLength(100);
            builder.Property(x => x.InquiryTaskWorkflowEnabled).IsRequired(true);
            builder.Property(x => x.InquiryTaskWorkflowSchemeIdentifier).HasMaxLength(100);

            builder.HasOne(x => x.UICSVSetting)
               .WithOne()
               .HasForeignKey<UICSVSetting>(e => e.ID);

            builder.HasOne(x => x.UIADSetting)
                .WithOne()
                .HasForeignKey<UIADSetting>(x => x.ID);

            builder.Property(x => x.RowVersion)
                .IsRowVersion()
                .IsRequired(true)
                .HasColumnType("timestamp");


            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<UISetting> builder);

        protected abstract string PrimaryKeyName { get; }
    }
}
