using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;

namespace InfraManager.BLL.ServiceCatalogue.OperationalLevelAgreements;

public class OperationalLevelAgreementLoader : ILoadEntity<int, OperationalLevelAgreement>,
    ISelfRegisteredService<ILoadEntity<int, OperationalLevelAgreement>>
{
    private readonly IFinder<OperationalLevelAgreement> _finder;

    public OperationalLevelAgreementLoader(IFinder<OperationalLevelAgreement> finder)
    {
        _finder = finder;
    }

    public Task<OperationalLevelAgreement> LoadAsync(int id, CancellationToken cancellationToken = default)
    {
        return _finder.With(x => x.TimeZone).With(x => x.CalendarWorkSchedule)
            .FindOrRaiseErrorAsync(id, cancellationToken);
    }
}