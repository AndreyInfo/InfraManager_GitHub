using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.MassIncidents;
using InfraManager.Core.Extensions;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.MassIncidents;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager;
using InfraManager.BLL;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class MassIncidentClient : ClientWithAuthorization
    {
        internal static string _url = "MassIncidents/";
        public MassIncidentClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<MassIncidentDetailsModel> GetAsync(Guid guid, Guid? userID = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await GetAsync<MassIncidentDetailsModel[]>($"{_url}?GlobalIdentifiers={guid}", userID, cancellationToken)).FirstOrDefault();
        }

        public async Task<MassIncidentDetailsModel> GetAsync(long number, Guid? userID = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<MassIncidentDetailsModel>($"{_url}{number}", userID, cancellationToken);
        }

        public async Task<MassIncidentDetailsModel[]> GetAsync(Guid referenceID, ObjectClass objectClass, Guid? userID = null, CancellationToken cancellationToken = default)
        {
            return await GetAsync<MassIncidentDetailsModel[]>($"{_url}?ReferenceID={referenceID}&ObjectClass={objectClass}", userID, cancellationToken);
        }

        public async Task<MassIncidentDetailsModel> SaveAsync(long id, MassIncidentData data, Guid? userID = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PatchAsync<MassIncidentDetailsModel, MassIncidentData>($"{_url}{id}", data, userID, cancellationToken);
        }

        public async Task<NoteListItemModel[]> GetNotesAsync(int massIncidentId, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<NoteListItemModel[]>(_url + $"{massIncidentId}/notes", userId, cancellationToken);
        }

        public async Task<AllMassIncidentsReportItemModel[]> GetListAsync(ListFilter listFilter= null, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PostAsync<AllMassIncidentsReportItemModel[], ListFilter>(_url + "reports/allMassIncidents", listFilter, x => PreProcessRequestHeaders(x, userId), cancellationToken);
        }

        public async Task<MassIncidentReferenceDetails[]> GetCallsAsync(long number, Guid? userID = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<MassIncidentReferenceDetails[]>($"{_url}{number}/calls", userID, cancellationToken);
        }

        public async Task<MassIncidentReferenceDetails[]> GetProblemsAsync(long number, Guid? userID = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<MassIncidentReferenceDetails[]>($"{_url}{number}/problems", userID, cancellationToken);
        }

        public async Task<MassIncidentReferenceDetails[]> GetChangeRequestsAsync(long number, Guid? userID = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<MassIncidentReferenceDetails[]>($"{_url}{number}/changeRequests", userID, cancellationToken);
        }

        public async Task<NoteListItemModel[]> GetNotesAsync(long number, Guid? userID = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<NoteListItemModel[]>($"{_url}{number}/notes", userID, cancellationToken);
        }

        public async Task<ServiceReferenceModel[]> GetAffectedServicesAsync(long number, Guid? userID = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<ServiceReferenceModel[]>($"{_url}{number}/affectedServices", userID, cancellationToken);
        }

        public async Task<DependencyDetails[]> GetDependenciesAsync(long id)
        {
            return new DependencyDetails[] { };
        }

        public async Task<MassIncidentReferenceDetails> PostReferenceAsync(long massIncidentID, Guid referenceID, ObjectClass classID, Guid? userID = null, CancellationToken cancellationToken = default)
        {
            return await PostAsync<MassIncidentReferenceDetails, MassIncidentReferenceDataModel>(
                ReferenceUri(massIncidentID, classID), 
                new MassIncidentReferenceDataModel { ReferenceID = referenceID, }, 
                userID, 
                cancellationToken);
        }

        private static string ReferenceUri(long massIncidentID, ObjectClass classID)
        {
            var apiUrl = classID switch
            {
                ObjectClass.Call => "Calls",
                ObjectClass.Problem => "Problems",
                ObjectClass.ChangeRequest => "ChangeRequests",
                _ => throw new ArgumentOutOfRangeException(nameof(classID), classID, $"Ассоциация массового инцидента не поддерживается для объекта класса '{classID}'."),
            };
            return $"{_url}{massIncidentID}/{apiUrl}";
        }
    }
}
