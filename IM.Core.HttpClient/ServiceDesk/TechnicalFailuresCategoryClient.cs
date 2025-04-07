using InfraManager.BLL.ServiceDesk.HandlingTechnicalFailures;
using InfraManager.BLL.ServiceDesk.MassIncidents.TechnicalFailureCategories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.ServiceDesk
{    
    public class TechnicalFailuresCategoryClient : ClientWithAuthorization
    {
        internal static string _url = "TechnicalFailuresCategories/";
        internal static string _handlingUrl = "HandlingTechnicalFailures/";
        public TechnicalFailuresCategoryClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<TechnicalFailureCategoryDetails> GetAsync(int number, Guid? userID = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<TechnicalFailureCategoryDetails>($"{_url}{number}", userID, cancellationToken);
        }

        public async Task<TechnicalFailureCategoryDetails[]> GetAsync(TechnicalFailureCategoryFilter filter, Guid? userID = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<TechnicalFailureCategoryDetails[], TechnicalFailureCategoryFilter>(_url, filter, userID, cancellationToken);
        }

        public async Task<HandlingTechnicalFailureDetails[]> GetHandlingAsync(int categoryID, Guid serviceID, Guid? userID = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<HandlingTechnicalFailureDetails[], HandlingTechnicalFailureFilter>(
                $"{_handlingUrl}", 
                new HandlingTechnicalFailureFilter
                {
                    ServiceID = serviceID, 
                    CategoryID = categoryID,
                }, 
                userID, 
                cancellationToken);
        }

    }
}
