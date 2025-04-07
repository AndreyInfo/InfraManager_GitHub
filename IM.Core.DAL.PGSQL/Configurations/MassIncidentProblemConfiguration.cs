using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using System;
using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class MassIncidentProblemConfiguration : ManyToManyConfigurationBase<MassIncident, Problem, int, Guid>
    {
        public static string MassIncidentIDColumnName = "mass_incident_id";

        public MassIncidentProblemConfiguration() 
            : base(
                  tableName: "mass_incident_problem",
                  schemaName: Options.Scheme,
                  primaryKeyColumnName: "id",
                  parentKeyColumnName: MassIncidentIDColumnName,
                  foreignKeyColumnName: "problem_id",
                  primaryKeyConstraintName: "pk_mass_incident_problem",
                  uniqueKeyName: "uk_mass_incident_problem",
                  foreignKeyConstraintName: "fk_mass_incident_problem_problem",
                  parentForeignKeyConstraintName: "fk_mass_incident_problem_massive_incident",
                  x => x.Problems,
                  DeleteBehavior.Cascade,
                  DeleteBehavior.Cascade
                  )
        {
        }
    }
}
