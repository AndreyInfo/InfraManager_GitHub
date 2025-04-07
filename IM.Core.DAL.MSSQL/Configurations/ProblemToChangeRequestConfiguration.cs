using System;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal class ProblemToChangeRequestConfiguration : ManyToManyConfigurationBase<Problem, ChangeRequest, Guid, Guid>
{
    public ProblemToChangeRequestConfiguration()
        : base(
            tableName: "ProblemToChangeRequest",
            schemaName: "dbo",
            primaryKeyColumnName: "ID",
            parentKeyColumnName: "ProblemID",
            foreignKeyColumnName: "ChangeRequestID",
            primaryKeyConstraintName: "PK_ProblemToChangeRequest",
            uniqueKeyName: "UQ_ProblemToChangeRequest",
            foreignKeyConstraintName: "FK_ProblemToChangeRequest_ChangeRequest",
            parentForeignKeyConstraintName: "FK_ProblemToChangeRequest_Problem",
            parentNavigationCollection: x => x.ChangeRequests)
    {
    }
}