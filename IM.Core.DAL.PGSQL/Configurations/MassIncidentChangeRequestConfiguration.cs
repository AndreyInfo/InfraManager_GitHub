using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using System;
using IM.Core.DAL.Postgres;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class MassIncidentChangeRequestConfiguration : ManyToManyConfigurationBase<MassIncident, ChangeRequest, int, Guid>
    {
        public static string MassIncidentIDColumnName = "mass_incident_id";

        public MassIncidentChangeRequestConfiguration()
            : base(
                  tableName: "mass_incident_change_request",
                  schemaName: Options.Scheme,
                  primaryKeyColumnName: "id",
                  parentKeyColumnName: MassIncidentIDColumnName,
                  foreignKeyColumnName: "change_request_id",
                  primaryKeyConstraintName: "pk_mass_incident_change_request",
                  uniqueKeyName: "uk_mass_incident_change_request",
                  foreignKeyConstraintName: "fk_mass_incident_change_request_change_request",
                  parentForeignKeyConstraintName: "fk_mass_incident_change_request_mass_incident",
                  x => x.ChangeRequests)
        {
        }
    }
}
