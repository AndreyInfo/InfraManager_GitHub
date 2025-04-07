using InfraManager.DAL;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class MassIncidentsInformationChannelLookupQuery : ILookupQuery
    {
        private readonly IMassIncidentInformationChannelBLL _lookupBLL;

        public MassIncidentsInformationChannelLookupQuery(IMassIncidentInformationChannelBLL lookupBLL)
        {
            _lookupBLL = lookupBLL;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var values = await _lookupBLL.AllAsync(cancellationToken);

            return values.Select(x => new ValueData { ID = x.ID.ToString(), Info = x.Name }).ToArray();
        }
    }
}
