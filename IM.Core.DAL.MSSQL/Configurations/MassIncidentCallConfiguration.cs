using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.EntityFrameworkCore;
using System;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class MassIncidentCallConfiguration : ManyToManyConfigurationBase<MassIncident, Call, int, Guid>
    {
        public static string MassIncidentIDColumnName = "MassIncidentID";

        public MassIncidentCallConfiguration() 
            : base(
                  tableName: "MassIncidentCall",
                  schemaName: "dbo",
                  primaryKeyColumnName: "ID",
                  parentKeyColumnName: MassIncidentIDColumnName,
                  foreignKeyColumnName: "CallID",
                  primaryKeyConstraintName: "PK_MassIncidentCall",
                  uniqueKeyName: "UK_MassIncidentCall",
                  foreignKeyConstraintName: "FK_MassIncidentCall_Call",
                  parentForeignKeyConstraintName: "FK_MassIncidentCall_MassiveIncident",
                  x => x.Calls,
                  DeleteBehavior.Cascade, 
                  DeleteBehavior.Cascade)
        {
        }
    }
}
