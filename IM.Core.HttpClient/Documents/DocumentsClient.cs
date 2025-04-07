using InfraManager.WebApi.Contracts.Models.Documents;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.Documents
{
    public class DocumentsClient : ClientWithAuthorization
    {
        internal static string _url = "document/";
        public DocumentsClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<DocumentInfoDetailsModel> GetAsync(Guid documentID, Guid? userID = null, CancellationToken cancellationToken = default)
        {
            return await GetAsync<DocumentInfoDetailsModel>($"{_url}{documentID}", userID, cancellationToken);
        }

        public async Task<DocumentInfoDetailsModel[]> GetByObjectAsync(Guid objectID,
            CancellationToken cancellationToken = default)
        {
            return await GetAsync<DocumentInfoDetailsModel[]>($"/api/object/{objectID}/documents", cancellationToken: cancellationToken);
        }
    }
}
