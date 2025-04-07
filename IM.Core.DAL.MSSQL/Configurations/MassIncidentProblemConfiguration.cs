using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.EntityFrameworkCore;
using System;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class MassIncidentProblemConfiguration : ManyToManyConfigurationBase<MassIncident, Problem, int, Guid>
    {
        public static string MassIncidentIDColumnName = "MassIncidentID";

        public MassIncidentProblemConfiguration() 
            : base(
                  tableName: "MassIncidentProblem",
                  schemaName: "dbo",
                  primaryKeyColumnName: "ID",
                  parentKeyColumnName: MassIncidentIDColumnName,
                  foreignKeyColumnName: "ProblemID",
                  primaryKeyConstraintName: "PK_MassIncidentProblem",
                  uniqueKeyName: "UK_MassIncidentProblem",
                  foreignKeyConstraintName: "FK_MassIncidentProblem_Problem",
                  parentForeignKeyConstraintName: "FK_MassIncidentProblem_MassIncident",
                  x => x.Problems,
                  DeleteBehavior.Cascade,
                  DeleteBehavior.Cascade)
        {
        }
    }
}
