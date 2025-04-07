using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.Problems;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.Problems;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class ProblemsClient : ClientWithAuthorization
    {
        private const string Url = "Problems/";

        public ProblemsClient(string baseUrl) : base(baseUrl)
        {
        }

        //  TODO: Раскоментировать и поправить модели по готовности WebAPI
        public async Task<ProblemDetailsModel> GetAsync(Guid guid, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<ProblemDetailsModel>($"{Url}{guid}", userId, cancellationToken);
        }

        public async Task<ProblemDetailsModel> SaveAsync(Guid guid, ProblemData Problem, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PutAsync<ProblemDetailsModel, ProblemData>($"{Url}{guid}", Problem, userId, cancellationToken);
        }

        public async Task<ProblemDetailsModel> AddAsync(ProblemData Problem, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PostAsync<ProblemDetailsModel, ProblemData>(Url, Problem, userId, cancellationToken);
        }
        
        public async Task<ProblemListItem[]> GetListAsync(ListFilter listFilter = null, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetListAsync<ProblemListItem[]>(Url + "reports/allproblems", listFilter, userId, cancellationToken);
        }
        
        public async Task<ProblemDetailsModel[]> GetListAsync(ProblemListFilter listFilter = null, Guid? userId = null, CancellationToken cancellationToken = default)
        {
            return await GetAsync<ProblemDetailsModel[], ProblemListFilter>(Url, listFilter, x => PreProcessRequestHeaders(x, userId), cancellationToken);
        }

        public async Task<NoteListItemModel[]> GetNotesAsync(Guid problemId, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<NoteListItemModel[]>(Url + $"{problemId}/notes", userId, cancellationToken);
        }

        public async Task<DependencyDetails[]> GetDependenciesAsync(Guid callID, Guid? userID = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<DependencyDetails[]>(Url + $"{callID}/dependencies", userID, cancellationToken);
        }

        public async Task<ProblemToChangeRequestReferenceDetails> PostAsync(
            Guid problemID,
            ProblemToChangeRequestReferenceDataModel data,
            Guid? userID = null,
            CancellationToken cancellationToken = default)
        {
            var uri = $"{Url}{problemID}/ChangeRequests";
            return await PostAsync<ProblemToChangeRequestReferenceDetails, ProblemToChangeRequestReferenceDataModel>(uri, data, userID, cancellationToken);
        }
    }
}
