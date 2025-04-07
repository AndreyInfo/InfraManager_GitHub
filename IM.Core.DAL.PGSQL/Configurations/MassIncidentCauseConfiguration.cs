using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class MassIncidentCauseConfiguration : MassIncidentCauseConfigurationBase
    {
        protected override string PrimaryKeyName => "fk_mass_incident_cause";
        protected override string IMObjIDUniqueKeyName => "uk_technical_failures_category_im_obj_id";

        protected override void ConfigureDataProvider(EntityTypeBuilder<MassIncidentCause> builder)
        {
            builder.ToTable("mass_incident_cause", Options.Scheme);

            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.Removed).HasColumnName("removed");
            builder.HasXminRowVersion(x => x.RowVersion);
            builder.Property(x => x.IMObjID).HasColumnName("im_obj_id").HasDefaultValueSql("gen_random_uuid()");
        }
    }
}
