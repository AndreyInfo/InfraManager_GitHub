using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    public class UISettingsConfiguration : UISettingConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_UISetting";

        protected override void ConfigureDatabase(EntityTypeBuilder<UISetting> builder)
        {
            builder.ToTable("UISetting", "dbo");

            builder.Property(x => x.ID)
                .HasColumnName("ID");
            builder.Property(x => x.Name)
                .HasColumnName("Name");
            builder.Property(x => x.Note)
                .HasColumnName("Note");
            builder.Property(x => x.ProviderType)
                .HasColumnName("ProviderType");
            builder.Property(x => x.ObjectType)
                .HasColumnName("ObjectType");
            builder.Property(x => x.LocationMode)
                .HasColumnName("LocationMode");
            builder.Property(x => x.RestoreRemovedUsers)
                .HasColumnName("RestoreRemovedUsers");
            builder.Property(x => x.UpdateLocation)
                .HasColumnName("UpdateLocation");
            builder.Property(x => x.UpdateSubdivision)
                .HasColumnName("UpdateSubdivision");
            builder.Property(x => x.OrganizationComparison)
                .HasColumnName("OrganizationComparison");
            builder.Property(x => x.SubdivisionComparison)
                .HasColumnName("SubdivisionComparison");
            builder.Property(x => x.UserComparison)
                .HasColumnName("UserComparison");
            builder.Property(x => x.LocationItemID)
                .HasColumnName("LocationItemID");
            builder.Property(x => x.SubdivisionDefaultOrganizationItemClassID)
                .HasColumnName("SubdivisionDefaultOrganizationItemClassID");
            builder.Property(x => x.SubdivisionDefaultOrganizationItemID)
                .HasColumnName("SubdivisionDefaultOrganizationItemID");
            builder.Property(x => x.UserDefaultOrganizationItemID)
                .HasColumnName("UserDefaultOrganizationItemID");
            builder.Property(x => x.UserAddedWorkflowEnabled)
                .HasColumnName("UserAddedWorkflowEnabled");
            builder.Property(x => x.UserAddedWorkflowSchemeIdentifier)
                .HasColumnName("UserAddedWorkflowSchemeIdentifier");
            builder.Property(x => x.UserModifiedWorkflowEnabled)
                .HasColumnName("UserModifiedWorkflowEnabled");
            builder.Property(x => x.UserModifiedWorkflowSchemeIdentifier)
                .HasColumnName("UserModifiedWorkflowSchemeIdentifier");
            builder.Property(x => x.UserRemovedWorkflowEnabled)
                .HasColumnName("UserRemovedWorkflowEnabled");
            builder.Property(x => x.UserRemovedWorkflowSchemeIdentifier)
                .HasColumnName("UserRemovedWorkflowSchemeIdentifier");
            builder.Property(x => x.DivisionAddedWorkflowEnabled)
                .HasColumnName("DivisionAddedWorkflowEnabled");
            builder.Property(x => x.DivisionAddedWorkflowSchemeIdentifier)
                .HasColumnName("DivisionAddedWorkflowSchemeIdentifier");
            builder.Property(x => x.DivisionModifiedWorkflowEnabled)
                .HasColumnName("DivisionModifiedWorkflowEnabled");
            builder.Property(x => x.DivisionModifiedWorkflowSchemeIdentifier)
                .HasColumnName("DivisionModifiedWorkflowSchemeIdentifier");
            builder.Property(x => x.OrganizationAddedWorkflowEnabled)
                .HasColumnName("OrganizationAddedWorkflowEnabled");
            builder.Property(x => x.OrganizationAddedWorkflowSchemeIdentifier)
                .HasColumnName("OrganizationAddedWorkflowSchemeIdentifier");
            builder.Property(x => x.OrganizationModifiedWorkflowEnabled)
                .HasColumnName("OrganizationModifiedWorkflowEnabled");
            builder.Property(x => x.OrganizationModifiedWorkflowSchemeIdentifier)
                .HasColumnName("OrganizationModifiedWorkflowSchemeIdentifier");
            builder.Property(x => x.InquiryTaskWorkflowEnabled)
                .HasColumnName("InquiryTaskWorkflowEnabled");
            builder.Property(x => x.InquiryTaskWorkflowSchemeIdentifier)
                .HasColumnName("InquiryTaskWorkflowSchemeIdentifier");
            builder.Property(x => x.Removed).HasColumnName("Removed");
        }
    }
}
