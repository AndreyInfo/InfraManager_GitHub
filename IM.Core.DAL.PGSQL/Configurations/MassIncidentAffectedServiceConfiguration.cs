using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using System;
using IM.Core.DAL.Postgres;
using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class MassIncidentAffectedServiceConfiguration : ManyToManyConfigurationBase<MassIncident, Service, int, Guid>
    {
        public static string MassiveIncidentIDColumnName = "mass_incident_id";

        public MassIncidentAffectedServiceConfiguration()
            : base(
                  tableName: "mass_incident_affected_service",
                  schemaName: Options.Scheme,
                  primaryKeyColumnName: "id",
                  parentKeyColumnName: MassiveIncidentIDColumnName,
                  foreignKeyColumnName: "service_id",
                  primaryKeyConstraintName: "pk_mass_incident_affected_service",
                  uniqueKeyName: "uk_mass_incident_affected_service",
                  foreignKeyConstraintName: "fk_mass_incident_affected_service_service",
                  parentForeignKeyConstraintName: "fk_mass_incident_affected_service_mass_incident",
                  x => x.AffectedServices)
        {
        }
    }
}
