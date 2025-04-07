using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using System;
using IM.Core.DAL.Postgres;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class MassIncidentCallConfiguration : ManyToManyConfigurationBase<MassIncident, Call, int, Guid>
    {
        public static string MassIncidentIDColumnName = "mass_incident_id";

        public MassIncidentCallConfiguration() 
            : base(
                  tableName: "mass_incident_call",
                  schemaName: Options.Scheme,
                  primaryKeyColumnName: "id",
                  parentKeyColumnName: MassIncidentIDColumnName,
                  foreignKeyColumnName: "call_id",
                  primaryKeyConstraintName: "pk_mass_incident_call",
                  uniqueKeyName: "uk_mass_incident_call",
                  foreignKeyConstraintName: "fk_mass_incident_call_call",
                  parentForeignKeyConstraintName: "fk_mass_incident_call_massive_incident",
                  x => x.Calls)
        {
        }
    }
}
