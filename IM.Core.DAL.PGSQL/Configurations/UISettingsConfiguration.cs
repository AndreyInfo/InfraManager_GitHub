using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations;

public class UISettingsConfiguration : UISettingConfigurationBase
{
    protected override string PrimaryKeyName => "pk_ui_setting";

    protected override void ConfigureDatabase(EntityTypeBuilder<UISetting> builder)
    {
        builder.ToTable("ui_setting", Options.Scheme);

        builder.Property(x => x.ID)
            .HasColumnName("id");
        builder.Property(x => x.Name)
            .HasColumnName("name");
        builder.Property(x => x.Note).HasColumnName("note");
        builder.Property(x => x.ProviderType)
            .HasColumnName("provider_type");
        builder.Property(x => x.ObjectType)
            .HasColumnName("object_type");
        builder.Property(x => x.LocationMode)
            .HasColumnName("location_mode");
        builder.Property(x => x.RestoreRemovedUsers)
            .HasColumnName("restore_removed_users");
        builder.HasXminRowVersion(x => x.RowVersion);
        builder.Property(x => x.LocationItemID)
            .HasColumnName("location_item_id");
        builder.Property(x => x.UpdateLocation)
            .HasColumnName("update_location");
        builder.Property(x => x.UpdateSubdivision)
            .HasColumnName("update_subdivision");
        builder.Property(x => x.OrganizationComparison)
            .HasColumnName("organization_comparison");
        builder.Property(x => x.SubdivisionComparison)
            .HasColumnName("subdivision_comparison");
        builder.Property(x => x.UserComparison)
            .HasColumnName("user_comparison");
        builder.Property(x => x.SubdivisionDefaultOrganizationItemClassID)
            .HasColumnName("subdivision_default_organization_item_class_id");
        builder.Property(x => x.SubdivisionDefaultOrganizationItemID)
            .HasColumnName("subdivision_default_organization_item_id");
        builder.Property(x => x.UserDefaultOrganizationItemID)
            .HasColumnName("user_default_organization_item_id");
        builder.Property(x => x.UserAddedWorkflowSchemeIdentifier)
            .HasColumnName("user_added_workflow_scheme_identifier");
        builder.Property(x => x.UserAddedWorkflowEnabled)
            .HasColumnName("user_added_workflow_enabled");
        builder.Property(x => x.UserModifiedWorkflowEnabled)
            .HasColumnName("user_modified_workflow_enabled");
        builder.Property(x => x.UserModifiedWorkflowSchemeIdentifier)
            .HasColumnName("user_modified_workflow_scheme_identifier");
        builder.Property(x => x.UserRemovedWorkflowEnabled)
            .HasColumnName("user_removed_workflow_enabled");
        builder.Property(x => x.UserRemovedWorkflowSchemeIdentifier)
            .HasColumnName("user_removed_workflow_scheme_identifier");
        builder.Property(x => x.DivisionAddedWorkflowEnabled)
            .HasColumnName("division_added_workflow_enabled");
        builder.Property(x => x.DivisionAddedWorkflowSchemeIdentifier)
            .HasColumnName("division_added_workflow_scheme_identifier");
        builder.Property(x => x.DivisionModifiedWorkflowEnabled)
            .HasColumnName("division_modified_workflow_enabled");
        builder.Property(x => x.DivisionModifiedWorkflowSchemeIdentifier)
            .HasColumnName("division_modified_workflow_scheme_identifier");
        builder.Property(x => x.OrganizationAddedWorkflowEnabled)
            .HasColumnName("organization_added_workflow_enabled");
        builder.Property(x => x.OrganizationAddedWorkflowSchemeIdentifier)
            .HasColumnName("organization_added_workflow_scheme_identifier");
        builder.Property(x => x.OrganizationModifiedWorkflowEnabled)
            .HasColumnName("organization_modified_workflow_enabled");
        builder.Property(x => x.OrganizationModifiedWorkflowSchemeIdentifier)
            .HasColumnName("organization_modified_workflow_scheme_identifier");
        builder.Property(x => x.InquiryTaskWorkflowEnabled)
            .HasColumnName("inquiry_task_workflow_enabled");
        builder.Property(x => x.InquiryTaskWorkflowSchemeIdentifier)
            .HasColumnName("inquiry_task_workflow_scheme_identifier");
        builder.Property(x => x.Removed).HasColumnName("removed");
    }
}