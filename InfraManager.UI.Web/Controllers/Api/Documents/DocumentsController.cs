using AutoMapper;
using InfraManager.BLL.ServiceDesk;
using InfraManager.Web.Controllers;
using InfraManager.WebApi.Contracts.Models.Documents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentsController : BaseApiController
    {
        private readonly IDocumentBLL _documentBLL;
        private readonly IMapper _mapper;

        public DocumentsController(
            IDocumentBLL documentBLL,
            IMapper mapper)
        {
            _documentBLL = documentBLL;
            _mapper = mapper;
        }

        [HttpGet("/api/document/{documentID}")]
        public async Task<DocumentInfoDetailsModel> GetDocument(Guid documentID, CancellationToken cancellationToken)
        {
            var info = await _documentBLL.GetObjectDocumentAsync(documentID, cancellationToken);
            return _mapper.Map<DocumentInfoDetailsModel>(info);
        }

    }
}
