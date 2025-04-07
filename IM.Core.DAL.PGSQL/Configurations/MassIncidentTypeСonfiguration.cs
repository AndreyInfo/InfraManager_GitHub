using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class MassIncidentTypeСonfiguration : MassIncidentTypeConfigurationBase
    {
        protected override string NameUniqueKeyName => "ui_mass_incident_type_name";
        protected override string PrimaryKeyName => "pk_mass_incident_type";
        protected override string DefaultValueIMObjID => "gen_random_uuid()";
        protected override string FormForeignKeyName => "fk_mass_incident_type_form";

        protected override void ConfigureDataProvider(EntityTypeBuilder<MassIncidentType> builder)
        {
            builder.ToTable("mass_incident_type", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(x => x.FormID).HasColumnName("form_id");            
            builder.Property(x => x.Removed).HasColumnName("removed");
            builder.Property(x => x.WorkflowSchemeIdentifier).HasColumnName("workflow_scheme_identifier");
            builder.Property(x => x.IMObjID).HasColumnName("im_obj_id");

            builder.HasXminRowVersion(x => x.RowVersion);
        }
    }
}
