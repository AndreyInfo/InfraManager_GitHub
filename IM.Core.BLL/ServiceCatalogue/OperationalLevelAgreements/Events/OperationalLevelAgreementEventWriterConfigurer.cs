using Inframanager.BLL.Events;
using InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;

namespace InfraManager.BLL.ServiceCatalogue.OperationalLevelAgreements.Events;

public class OperationalLevelAgreementEventWriterConfigurer : IConfigureEventWriter<OperationalLevelAgreement,
        OperationalLevelAgreement>
{
    public void Configure(IEventWriter<OperationalLevelAgreement, OperationalLevelAgreement> writer)
    {
    }
}