using System;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;
using Microsoft.EntityFrameworkCore;

namespace IM.Core.DAL.Postgres.Configurations;

public class
    OperationalLeveAgreementServiceConfiguration : ManyToManyConfigurationBase<OperationalLevelAgreement, Service, int,
        Guid>
{
    public OperationalLeveAgreementServiceConfiguration() : base(
        tableName: "operational_level_agreement_service",
        schemaName: Options.Scheme,
        primaryKeyColumnName: "id",
        parentKeyColumnName: "operational_level_agreement_id",
        foreignKeyColumnName: "service_id",
        primaryKeyConstraintName: "pk_operational_level_agreement_service",
        uniqueKeyName: "uk_operational_level_agreement_service",
        foreignKeyConstraintName: "fk_operational_level_agreement_service",
        parentForeignKeyConstraintName: "fk_operational_level_agreement_service_operational_level_agreem",
        x => x.Services,
        DeleteBehavior.Cascade,
        DeleteBehavior.Cascade)
    {
    }
}