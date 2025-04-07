using InfraManager;
using InfraManager.BLL.ServiceDesk;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.Message
{

    public class NoteClient : ClientWithAuthorization
    {

        Dictionary<ObjectClass, string> Domains = new Dictionary<ObjectClass, string>();
        public NoteClient(string baseUrl) : base(baseUrl)
        {
            Domains.Add(ObjectClass.Call, "calls");
            Domains.Add(ObjectClass.WorkOrder, "workorders");
            Domains.Add(ObjectClass.ChangeRequest, "ChangeRequests");
            Domains.Add(ObjectClass.MassIncident, "massIncidents");
            Domains.Add(ObjectClass.Problem, "Problems");
        }

        public async Task<NoteListItemModel> AddAsync(NoteData note, int objectCalssID, Guid objectID, Guid? userID = null, CancellationToken cancellationToken = default(CancellationToken))
        {
           return await PostAsync<NoteListItemModel, NoteData>($"{Domains[(ObjectClass)objectCalssID]}/{objectID}/notes", note, userID, cancellationToken);
        }
        public async Task<NoteListItemModel> AddAsync(NoteData note, int objectCalssID, long objectID, Guid? userID = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PostAsync<NoteListItemModel, NoteData>($"{Domains[(ObjectClass)objectCalssID]}/{objectID}/notes", note, userID, cancellationToken);
        }
    }
}
