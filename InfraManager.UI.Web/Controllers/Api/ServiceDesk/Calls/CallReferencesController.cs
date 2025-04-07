using InfraManager.BLL.ServiceDesk.Calls;
using InfraManager.UI.Web.ResourceMapping;
using InfraManager.Web.SignalR;
using InfraManager.WebApi.Contracts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.Calls;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.Calls
{
    [Authorize]
    [Route("api")]
    public class CallReferencesController : ControllerBase
    {
        #region .ctor

        private readonly IServiceMapper<ObjectClass, ICallReferenceBLL> _services;
        private readonly IHubContext<EventHub> _hub;
        private readonly IMapper _mapper;

        public CallReferencesController(
            IServiceMapper<ObjectClass, ICallReferenceBLL> services,
            IHubContext<EventHub> hub,
            IMapper mapper)
        {
            _services = services;
            _hub = hub;
            _mapper = mapper;
        }

        #endregion

        #region Problems

        [HttpGet("calls/{callID:guid}/problems")]
        public async Task<ActionResult<CallReferenceData[]>> GetReferencedProblemsAsync(Guid callID, CancellationToken cancellationToken = default) =>
            await GetCallReferencesAsync(callID, ObjectClass.Problem, cancellationToken);

        [HttpGet("problems/{problemID:guid}/calls")]
        public async Task<ActionResult<CallReferenceData[]>> GetProblemReferencedCallsAsync(Guid problemID, CancellationToken cancellationToken) =>
            await GetReferenceCallsAsync(problemID, ObjectClass.Problem, cancellationToken);

        [HttpPost("calls/{callID:guid}/problems")]
        public async Task<CallReferenceData> PostProblemAsync(Guid callID, [FromBody] ObjectID data, CancellationToken cancellationToken = default) =>
            await PostReferenceAsync(callID, ObjectClass.Problem, data, cancellationToken);

        [HttpPost("problems/{problemID:guid}/calls")]
        public async Task<CallReferenceData> PostProblemCallAsync(Guid problemID, [FromBody] ObjectID data, CancellationToken cancellationToken = default) =>
            await PostCallAsync(problemID, ObjectClass.Problem, data, cancellationToken);

        [HttpDelete("calls/{callID:guid}/problems/{problemID:guid}")]
        public async Task DeleteProblemAsync(Guid callID, Guid problemID, CancellationToken cancellationToken = default) =>
            await DeleteReferenceAsync(callID, ObjectClass.Problem, problemID, cancellationToken);

        [HttpDelete("problems/{problemID:guid}/calls/{callID:guid}")]
        public async Task DeleteProblemCallAsync(Guid callID, Guid problemID, CancellationToken cancellationToken = default) =>
            await DeleteReferenceAsync(callID, ObjectClass.Problem, problemID, cancellationToken);

        [HttpGet("problems/{problemID:guid}/calls/references")] //TODO: изменить URL на problems/{objectID}/reports/referencedCalls (и это не REST api)
        public async Task<ActionResult<CallDetailsModel[]>> GetProblemReferencedCallsReportAsync(
            Guid problemID,
            [FromQuery] CallReferenceListFilter filter,
            CancellationToken cancellationToken = default) =>
            await GetReferencesReportAsync(ObjectClass.Problem, problemID, filter, cancellationToken);

        #endregion

        #region Change Requests


        [HttpGet("calls/{callID:guid}/changeRequests")]
        public async Task<ActionResult<CallReferenceData[]>> GetReferencedChangeRequestsAsync(Guid callID, CancellationToken cancellationToken = default) =>
            await GetCallReferencesAsync(callID, ObjectClass.ChangeRequest, cancellationToken);

        [HttpGet("changeRequests/{changeRequestID:guid}/calls")]
        public async Task<ActionResult<CallReferenceData[]>> GetChangeRequestCallsAsync(Guid changeRequestID, CancellationToken cancellationToken) =>
            await GetReferenceCallsAsync(changeRequestID, ObjectClass.ChangeRequest, cancellationToken);

        [HttpPost("calls/{callID:guid}/changeRequests")]
        public async Task<CallReferenceData> PostChangeRequestAsync(Guid callID, [FromBody] ObjectID data, CancellationToken cancellationToken = default) =>
            await PostReferenceAsync(callID, ObjectClass.ChangeRequest, data, cancellationToken);

        [HttpPost("changeRequests/{changeRequestID:guid}/calls")]
        public async Task<CallReferenceData> PostChangeRequestCallAsync(Guid changeRequestID, [FromBody] ObjectID data, CancellationToken cancellationToken = default) =>
            await PostCallAsync(changeRequestID, ObjectClass.ChangeRequest, data, cancellationToken);

        [HttpGet("changeRequests/{changeRequestID:guid}/calls/references")] //TODO: изменить URL на changeRequests/{objectID}/reports/referencedCalls (и это не REST api)
        public async Task<ActionResult<CallDetailsModel[]>> GetChangeRequestCallReferencesReportAsync(
            Guid problemID,
            [FromQuery] CallReferenceListFilter filter,
            CancellationToken cancellationToken = default) =>
            await GetReferencesReportAsync(ObjectClass.ChangeRequest, problemID, filter, cancellationToken);

        [HttpDelete("calls/{callID:guid}/changeRequests/{changeRequestID:guid}")]
        public async Task DeleteChangeRequestAsync(Guid callID, Guid changeRequestID, CancellationToken cancellationToken = default) =>
            await DeleteReferenceAsync(callID, ObjectClass.ChangeRequest, changeRequestID, cancellationToken);

        [HttpDelete("changeRequests/{changeRequestID:guid}/calls/{callID:guid}")]
        public async Task DeleteChangeRequestCallAsync(Guid callID, Guid changeRequestID, CancellationToken cancellationToken = default) =>
            await DeleteReferenceAsync(callID, ObjectClass.ChangeRequest, changeRequestID, cancellationToken);

        #endregion

        #region Reports

        private async Task<ActionResult<CallDetailsModel[]>> GetReferencesReportAsync(
            ObjectClass referenceClassID,
            Guid objectID,
            [FromQuery] CallReferenceListFilter filter,
            CancellationToken cancellationToken = default)
        {
            filter.ObjectID = objectID; 

            return _mapper.Map<CallDetailsModel[]>(
                await GetService(referenceClassID).GetReferencesAsync(filter, cancellationToken));
        }

        #endregion

        #region Common

        private async Task<ActionResult<CallReferenceData[]>> GetCallReferencesAsync(
            Guid callID,
            ObjectClass referenceClassID,
            CancellationToken cancellationToken = default)
        {
            return await GetService(referenceClassID).GetAsync(new CallReferenceListFilter { CallID = callID }, cancellationToken);
        }

        private async Task<ActionResult<CallReferenceData[]>> GetReferenceCallsAsync(
            Guid referenceID,
            ObjectClass referenceClassID,
            CancellationToken cancellationToken = default)
        {
            return await GetService(referenceClassID).GetAsync(new CallReferenceListFilter { ObjectID = referenceID }, cancellationToken);
        }

        private async Task<CallReferenceData> PostReferenceAsync(
            Guid callID,
            ObjectClass referenceClassID,
            [FromBody] ObjectID data,
            CancellationToken cancellationToken = default) =>
            await GetService(referenceClassID)
                .AddAsync(new CallReferenceData { CallID = callID, ObjectID = data.ID }, cancellationToken);

        private async Task<CallReferenceData> PostCallAsync(            
            Guid referenceID,
            ObjectClass referenceClassID,
            ObjectID data,
            CancellationToken cancellationToken = default) =>
            await GetService(referenceClassID)
                .AddAsync(new CallReferenceData { CallID = data.ID, ObjectID = referenceID }, cancellationToken);

        private async Task DeleteReferenceAsync(
            Guid callID,
            ObjectClass referenceClassID,
            Guid referenceID,
            CancellationToken cancellationToken = default) =>
            await GetService(referenceClassID)
                .DeleteAsync(new CallReferenceData { CallID = callID, ObjectID = referenceID }, cancellationToken);

        private ICallReferenceBLL GetService(ObjectClass classID)
        {
            return _services.Map(classID);
        }

        #endregion
    }
}
