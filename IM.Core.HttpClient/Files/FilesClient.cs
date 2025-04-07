using InfraManager.WebApi.Contracts.Models.Documents;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.Files
{
    public class FilesClient : ClientWithAuthorization
    {
        internal static string _url = "Files/";
        public FilesClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<Guid> AddAsync(DocumentFileModel model, Guid? userID = null, CancellationToken cancellationToken = default)
        {
            return await PostAsync<Guid, DocumentFileModel>($"{_url}data", model, userID, cancellationToken);
        }

        public async Task<DocumentFileModel> GetAsync(Guid documentID, Guid? userID = null, CancellationToken cancellationToken = default)
        {
            return await GetAsync<DocumentFileModel>($"{_url}{documentID}/data", userID, cancellationToken);
        }
    }
}
