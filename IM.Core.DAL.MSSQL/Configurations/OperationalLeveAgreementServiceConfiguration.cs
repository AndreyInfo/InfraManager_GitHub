using System;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;
using Microsoft.EntityFrameworkCore;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;
 
 public class OperationalLeveAgreementServiceConfiguration: ManyToManyConfigurationBase<OperationalLevelAgreement, Service, int,
     Guid>
 {
     public OperationalLeveAgreementServiceConfiguration() : base(
         tableName: "OperationLevelAgreementService",
         schemaName: Options.Scheme,
         primaryKeyColumnName: "ID",
         parentKeyColumnName:"OperationLevelAgreementID",
         foreignKeyColumnName:"ServiceID",
         primaryKeyConstraintName:"PK_OperationLevelAgreementService",
         uniqueKeyName:"UK_OperationLevelAgreementService",
         foreignKeyConstraintName:"FK_OperationLevelAgreementService_Service",
         parentForeignKeyConstraintName:"FK_OperationLevelAgreementService_OperationLevelAgreement",
         x => x.Services,
         DeleteBehavior.Cascade,
         DeleteBehavior.Cascade)
     {
     }
 }