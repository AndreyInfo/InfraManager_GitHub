using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using System;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class MassIncidentAffectedServiceConfiguration : ManyToManyConfigurationBase<MassIncident, Service, int, Guid>
    {
        public static string MassIncidentIDColumnName = "MassIncidentID";

        public MassIncidentAffectedServiceConfiguration()
            : base(
                  tableName: "MassIncidentAffectedService",
                  schemaName: "dbo",
                  primaryKeyColumnName: "ID",
                  parentKeyColumnName: MassIncidentIDColumnName,
                  foreignKeyColumnName: "ServiceID",
                  primaryKeyConstraintName: "PK_MassIncidentAffectedService",
                  uniqueKeyName: "UK_MassIncidentAffectedService",
                  foreignKeyConstraintName: "FK_MassIncidentAffectedService_Service",
                  parentForeignKeyConstraintName: "FK_MassIncidentAffectedService_MassIncident",
                  x => x.AffectedServices)
        {
        }
    }
}
