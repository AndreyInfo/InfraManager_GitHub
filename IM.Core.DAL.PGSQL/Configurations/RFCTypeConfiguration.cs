using IM.Core.DAL.Postgres;
using Inframanager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    public class RFCTypeConfiguration : RFCTypeConfigurationBase
    {
        protected override string IndexName => "ui_rfc_type_name";

        protected override void ConfigureDatabase(EntityTypeBuilder<ChangeRequestType> builder)
        {
            builder.ToTable("rfc_type", Options.Scheme);
            builder.HasKey(x => x.ID).HasName("pk_rfc_type");

            builder.Property(x => x.Icon).HasColumnName("icon");
            builder.Property(e => e.IconName).HasColumnName("icon_name");
            builder.Property(e => e.ID).HasColumnName("id");
            builder.Property(e => e.Name).HasColumnName("name");
            builder.Property(e => e.Removed).HasColumnName("removed");
            builder.Property(e => e.FormID).HasColumnName("form_id");
            builder.HasXminRowVersion(e => e.RowVersion);
            builder.Property(e => e.WorkflowSchemeIdentifier).HasColumnName("workflow_scheme_identifier");
        }
    }
}