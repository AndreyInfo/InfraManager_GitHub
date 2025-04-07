using InfraManager.BLL.Localization;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.Negotiations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    internal class NegotiationStatusLookupQuery : ILookupQuery
    {
        private readonly IReadonlyRepository<Negotiation> _repository;
        private readonly ILocalizeEnum<NegotiationStatus> _localizer;

        public NegotiationStatusLookupQuery(
            IReadonlyRepository<Negotiation> repository,
            ILocalizeEnum<NegotiationStatus> localizer)
        {
            _repository = repository;
            _localizer = localizer;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var queryResult = new List<ValueData>();
            foreach (var status in Enum.GetValues<NegotiationStatus>())
            {
                if (await _repository.AnyAsync(x => x.Status == status, cancellationToken))
                {
                    queryResult.Add(
                        new ValueData
                        {
                            ID = ((int)status).ToString(),
                            Info = await _localizer.LocalizeAsync(status, cancellationToken)
                        });
                }
            }

            return queryResult.ToArray();
        }
    }
}

