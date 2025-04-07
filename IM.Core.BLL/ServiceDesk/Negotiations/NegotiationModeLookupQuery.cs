using InfraManager.BLL.Localization;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.Negotiations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    internal class NegotiationModeLookupQuery : ILookupQuery
    {
        private readonly IReadonlyRepository<Negotiation> _repository;
        private readonly ILocalizeEnum<NegotiationMode> _localizer;

        public NegotiationModeLookupQuery(
            IReadonlyRepository<Negotiation> repository,
            ILocalizeEnum<NegotiationMode> localizer)
        {
            _repository = repository;
            _localizer = localizer;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var queryResult = new List<ValueData>();
            foreach(var mode in Enum.GetValues<NegotiationMode>())
            {
                if (await _repository.AnyAsync(x => x.Mode == mode, cancellationToken))
                {
                    queryResult.Add(
                        new ValueData 
                        { 
                            ID = ((int)mode).ToString(), 
                            Info = await _localizer.LocalizeAsync(mode, cancellationToken) 
                        });
                }
            }

            return queryResult.ToArray();
        }
    }
}

