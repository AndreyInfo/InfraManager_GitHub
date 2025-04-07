using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.EntityFrameworkCore;
using System;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class MassIncidentChangeRequestConfiguration : ManyToManyConfigurationBase<MassIncident, ChangeRequest, int, Guid>
    {
        public static string MassIncidentIDColumnName = "MassIncidentID";

        public MassIncidentChangeRequestConfiguration()
            : base(
                  tableName: "MassIncidentChangeRequest",
                  schemaName: "dbo",
                  primaryKeyColumnName: "ID",
                  parentKeyColumnName: MassIncidentIDColumnName,
                  foreignKeyColumnName: "ChangeRequestID",
                  primaryKeyConstraintName: "PK_MassIncidentChangeRequest",
                  uniqueKeyName: "UK_MassIncidentChangeRequest",
                  foreignKeyConstraintName: "FK_MassIncidentChangeRequest_ChangeRequest",
                  parentForeignKeyConstraintName: "FK_MassIncidentChangeRequest_MassIncident",
                  x => x.ChangeRequests,
                  DeleteBehavior.Cascade,
                  DeleteBehavior.Cascade)
        {
        }
    }
}
