using System;
using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;

namespace InfraManager.DAL.Postgres.Configurations;

internal class ProblemToChangeRequestConfiguration : ManyToManyConfigurationBase<Problem, ChangeRequest, Guid, Guid>
{
    public ProblemToChangeRequestConfiguration()
        : base(
            tableName: "problem_to_change_request",
            schemaName: Options.Scheme,
            primaryKeyColumnName: "id",
            parentKeyColumnName: "problem_id",
            foreignKeyColumnName: "change_request_id",
            primaryKeyConstraintName: "pk_problem_to_change_request",
            uniqueKeyName: "uq_problem_to_change_request",
            foreignKeyConstraintName: "fk_problem_to_change_request_change_request",
            parentForeignKeyConstraintName: "fk_problem_to_change_request_problem",
            parentNavigationCollection: x => x.ChangeRequests)
    {
    }
}