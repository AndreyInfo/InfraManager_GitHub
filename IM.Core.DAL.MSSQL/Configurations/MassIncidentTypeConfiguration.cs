using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class MassIncidentTypeConfiguration : MassIncidentTypeConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_MassIncident";
        protected override string NameUniqueKeyName => "UI_Mass_incident_type_name";
        protected override string DefaultValueIMObjID => "NEWID()";
        protected override string FormForeignKeyName => "FK_MassIncidentType_Form";

        protected override void ConfigureDataProvider(EntityTypeBuilder<MassIncidentType> builder)
        {
            builder.ToTable("MassIncidentType", Options.Scheme);

            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.WorkflowSchemeIdentifier).HasColumnName("WorkflowSchemeIdentifier");
            builder.Property(x => x.FormID).HasColumnName("FormID");
            builder.Property(x => x.Removed).HasColumnName("Removed");
            builder.Property(x => x.RowVersion).IsRowVersion().HasColumnName("RowVersion");
            builder.Property(x => x.IMObjID).HasColumnName("IMObjID");
        }
    }
}
