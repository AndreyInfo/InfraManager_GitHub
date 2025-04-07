using Inframanager.BLL.ListView;
using System.Threading.Tasks;
using System.Threading;
using Inframanager.BLL;
using InfraManager.DAL.ServiceDesk.Negotiations;
using System;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    public interface IReadNegotiationBLL
    {
        /// <summary>
        /// Список "На согласовании"
        /// </summary>
        /// <param name="filterBy">Ссылка на фильтр</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив данных списка</returns>
        Task<NegotiationListItem[]> GetReportAsync(
            ListViewFilterData<NegotiationListFilter> filterBy,
            CancellationToken cancellationToken = default);
        Task<NegotiationDetails> DetailsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<NegotiationDetails[]> GetDetailsArrayAsync(NegotiationListFilter filterBy, CancellationToken cancellationToken = default);
        Task<NegotiationDetails[]> GetDetailsPageAsync(NegotiationListFilter filterBy, ClientPageFilter<Negotiation> pageFilter, CancellationToken cancellationToken = default);
    }
}
