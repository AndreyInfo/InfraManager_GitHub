using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk.Calls;
using InfraManager.BLL.ServiceDesk.Quality;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class QualityControlClient : ClientWithAuthorization
    {
        internal static string _url = "QualityControl/";
        public QualityControlClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<QualityControlDetails> AddAsync(QualityControlData data, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PostAsync<QualityControlDetails, QualityControlData>($"{_url}", data, userId, cancellationToken);
        }

        public async Task<DateTime?> GetLastByCallAsync(Guid callId, Guid? userId = null, CancellationToken cancellationToken = default)
        {
            return await GetAsync<DateTime?>($"{_url}last-by-call?callId={callId}", userId, cancellationToken);
        }

    }
}
