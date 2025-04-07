using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.Negotiations;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    internal class ReadNegotiationBLL : IReadNegotiationBLL, ISelfRegisteredService<IReadNegotiationBLL>
    {
        #region .ctor

        private readonly IListViewBLL<NegotiationListItem, NegotiationListFilter> _dataListBuilder;
        private readonly IGetEntityBLL<Guid, Negotiation, NegotiationDetails> _detailsLoader;
        private readonly IGetEntityArrayBLL<Guid, Negotiation, NegotiationDetails, NegotiationListFilter> _arrayLoader;

        public ReadNegotiationBLL(
            IListViewBLL<NegotiationListItem, NegotiationListFilter> dataListBuilder,
            IGetEntityBLL<Guid, Negotiation, NegotiationDetails> detailsLoader,
            IGetEntityArrayBLL<Guid, Negotiation, NegotiationDetails, NegotiationListFilter> arrayLoader)
        {
            _dataListBuilder = dataListBuilder;
            _detailsLoader = detailsLoader;
            _arrayLoader = arrayLoader;
        }

        #endregion

        #region Get negotiations

        public Task<NegotiationDetails> DetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _detailsLoader.DetailsAsync(id, cancellationToken);
        }

        public async Task<NegotiationDetails[]> GetDetailsArrayAsync(NegotiationListFilter filterBy, CancellationToken cancellationToken = default)
        {
            return await _arrayLoader.ArrayAsync(filterBy, cancellationToken);
        }

        public async Task<NegotiationDetails[]> GetDetailsPageAsync(NegotiationListFilter filterBy, ClientPageFilter<Negotiation> pageFilter, CancellationToken cancellationToken = default)
        {
            return await _arrayLoader.PageAsync(filterBy, pageFilter, cancellationToken);
        }

        #endregion

        #region Reports

        public Task<NegotiationListItem[]> GetReportAsync(
            ListViewFilterData<NegotiationListFilter> filterBy,
            CancellationToken cancellationToken = default)
        {
            return _dataListBuilder.BuildAsync(filterBy, cancellationToken);
        }

        #endregion
    }
}
