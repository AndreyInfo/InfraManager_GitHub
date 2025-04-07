using AutoMapper.QueryableExtensions.Impl;
using InfraManager.BLL.ServiceDesk;
using InfraManager.UI.Web.Helpers;
using InfraManager.Web.BLL.Helpers;
using InfraManager.Web.Controllers;
using InfraManager.Web.DTL.Repository;
using InfraManager.Web.Helpers;
using InfraManager.Web.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api
{
    [Route("api/DocumentReferences")]
    [ApiController]
    [Authorize]
    public class DocumentReferencesController : BaseApiController
    {
        private readonly IDocumentReferenceBLL _documentReferenceBLL;
        private readonly IHubContext<EventHub> _hubContext;

        public DocumentReferencesController(IDocumentReferenceBLL documentReferenceBLL, IHubContext<EventHub> hubContext)
        {
            _documentReferenceBLL = documentReferenceBLL;
            _hubContext = hubContext;
        }

        [HttpPost("{classID}/{entityID}/documents")]
        public async Task<DocumentReferenceDetails[]> PostAsync(
            ObjectClass classID,
            Guid entityID,
            [FromForm] Guid[] docID,
            CancellationToken cancellationToken)
        {
            var details = await _documentReferenceBLL.AddReferencesAsync(classID, entityID, docID, cancellationToken);
            ObjectUpdated(entityID, classID);
            return details;
        }

        [HttpDelete("{classID}/{entityID}/documents/{docID}")]
        public async Task DeleteAsync(
            ObjectClass classID,
            Guid entityID,
            Guid docID,
            CancellationToken cancellationToken)
        {
            await _documentReferenceBLL.DeleteReferenceAsync(classID, entityID, docID, cancellationToken);
            ObjectUpdated(entityID, classID);
        }

        private void ObjectUpdated(Guid entityID, ObjectClass classID)
        {
            EventHub.ObjectUpdated(
                _hubContext,
                (int)classID,
                entityID,
                null,
                HttpContext.GetRequestConnectionID());
        }
    }
}
