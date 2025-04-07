using InfraManager.BLL.KB;
using InfraManager.BLL.KnowledgeBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.KnowlageBase
{
    public class KBClient : ClientWithAuthorization
    {
        internal static string _url = "kb/";
        public KBClient(string baseUrl) : base(baseUrl)
        {
        }
        public async Task<KBArticleFolderDetails[]> GetFolderHierarchyAsync(FolderFilter folderFilter = null, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetListAsync<KBArticleFolderDetails[], FolderFilter>($"{_url}FolderHierarchy", folderFilter, userId, cancellationToken);
        }

        public async Task<KBArticleShortDetails[]> GetFolderArticlesAsync(Guid folderId, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetListAsync<KBArticleShortDetails[]>($"{_url}Articles?folderID={folderId}", userId, cancellationToken);
        }

        public async Task<KBArticleDetails> GetAsync(Guid id, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<KBArticleDetails>($"{_url}Article?kbaId={id}", userId, cancellationToken);
        }



    }
}
