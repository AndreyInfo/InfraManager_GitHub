using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;

namespace InfraManager.DAL.ServiceDesk.MassIncidents
{
    internal class MassIncidentOLALookupQuery : ILookupQuery
    {
        private readonly DbSet<MassIncident> _massiveIncidents;
        private readonly DbSet<OperationalLevelAgreement> _agreements;

        public MassIncidentOLALookupQuery(DbSet<MassIncident> massiveIncidents, DbSet<OperationalLevelAgreement> agreements)
        {
            _massiveIncidents = massiveIncidents;
            _agreements = agreements;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var values = await _agreements.Where(ola => _massiveIncidents.Any(massIncident => massIncident.OperationalLevelAgreementID == ola.ID)).ToArrayAsync(cancellationToken);
            return values.Select(x => new ValueData { ID = x.ID.ToString(), Info = x.Name }).ToArray();
        }
    }
}
