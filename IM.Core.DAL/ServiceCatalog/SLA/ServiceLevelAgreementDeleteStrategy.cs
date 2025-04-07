using System;
using System.Linq;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.ServiceCatalog.SLA;

public class ServiceLevelAgreementDeleteStrategy : IDeleteStrategy<ServiceLevelAgreement>,
    ISelfRegisteredService<IDeleteStrategy<ServiceLevelAgreement>>
{
    private readonly IRepository<SLAReference> _serviceLevelAgreementsReferences;
    private readonly DbSet<ServiceLevelAgreement> _serviceLevelAgreements;

    public ServiceLevelAgreementDeleteStrategy(IRepository<SLAReference> serviceLevelAgreementsReferences,
        DbSet<ServiceLevelAgreement> serviceLevelAgreements)
    {
        _serviceLevelAgreementsReferences = serviceLevelAgreementsReferences;
        _serviceLevelAgreements = serviceLevelAgreements;
    }

    public void Delete(ServiceLevelAgreement entity)
    {
        var references = _serviceLevelAgreementsReferences.Where(x => x.SLAID == entity.ID).ToArray();
        references.ForEach(x => _serviceLevelAgreementsReferences.Delete(x));
        _serviceLevelAgreements.Remove(entity);
    }
}