using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using InfraManager;
using InfraManager.WebApi.Contracts.Models.Documents;

namespace IM.Core.HttpClient.ServiceDesk;

public class DocumentReferencesClient : ClientWithAuthorization
{
    public DocumentReferencesClient(string baseUrl)
        : base(baseUrl)
    {
    }

    public async Task<DocumentReferenceDetailsModel[]> PostAsync(
        ObjectClass classID,
        Guid entityID,
        Guid[] docID,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"DocumentReferences/{(int) classID}/{entityID}/documents";
        var response = await SendRequestAsync<DocumentReferenceDetailsModel[]>(
            () =>
            {
                var data = docID.Select(id => new KeyValuePair<string, string>("docID", id.ToString()));
                var request = new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = new FormUrlEncodedContent(data), };
                PreProcessRequestHeaders(request.Headers, null);
                return request;
            }, cancellationToken);
        return response;
    }
}