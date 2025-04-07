using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.Web.Controllers;
using InfraManager.BLL.FormBuilder;
using System;
using System.IO;
using System.Text;
using InfraManager.BLL.FormBuilder.Contracts;
using InfraManager.UI.Web.Models.FormBuilder;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;

namespace InfraManager.UI.Web.Controllers.Api.FormBuilder
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FullFormsController : BaseController
    {
        private readonly IFullFormBLL _formBuilderFullForm;
        private readonly IFormBuilderSettingsBLL _settingsService;
        public FullFormsController(
            IFullFormBLL formBuilderFullForm,
            IFormBuilderSettingsBLL settingsService)
        {
            _formBuilderFullForm = formBuilderFullForm;
            _settingsService = settingsService;
        }

        [HttpGet("{formID}")]
        public async Task<FormBuilderFullFormDetails> GetAsync(Guid formID,
            CancellationToken cancellationToken = default)
        {
            return await _formBuilderFullForm.GetAsync(formID, cancellationToken);
        }

        [HttpGet]
        public async Task<FormBuilderFullFormDetails> GetAsync([FromQuery] GetFormBuilderRequest request, CancellationToken cancellationToken = default)
        {
            return await _formBuilderFullForm.GetAsync(request.ClassID, request.ObjectID, cancellationToken);
        }

        [HttpPut("{formID}")]
        public async Task<Guid> PutAsync([FromRoute] Guid formID, [FromBody] FormBuilderFullFormData data,
            CancellationToken cancellationToken = default)
        {
            return await _formBuilderFullForm.SaveAsync(formID, data, cancellationToken);
        }

        [HttpPost("{formID}/Clone")]
        public async Task<Guid> CloneAsync([FromRoute] Guid formID, [FromBody] FormBuilderCloneRequest request,
            CancellationToken cancellationToken = default)
        {
            return await _formBuilderFullForm.CloneAsync(formID, request.Name, request.Identifier, request.Description,
                cancellationToken);
        }

        [HttpPost("{formID}/Export")]
        public async Task<FileResult> ExportAsync([FromRoute] Guid formID,
            CancellationToken cancellationToken = default)
        {
            var result = await _formBuilderFullForm.ExportAsync(formID, cancellationToken);

            var response = File(new MemoryStream(Encoding.UTF8.GetBytes(result ?? "")),
                "application/octet-stream");

            Response.Headers.ContentDisposition =
                "attachment; filename=FormBuilder.json";

            return response;
        }


        [HttpPost("Import")]
        public async Task ImportAsync([FromBody] ImportRequest formBuilderJson,
            CancellationToken cancellationToken = default)
        {
            await _formBuilderFullForm.ImportAsync(formBuilderJson.FormBuilderJson, cancellationToken);
        }
        
        [HttpGet("{formID:guid}/settings")]
        public async Task<FormBuilderFormSettingDetails> GetSettingsAsync(Guid formID, CancellationToken cancellationToken = default)
        {
            return await _settingsService.GetSettingsAsync(formID, cancellationToken);
        }

        [HttpPost("{formID:guid}/settings")]
        public async Task SaveSettingsAsync(Guid formID, [FromBody] FormBuilderFormSettingData data, CancellationToken cancellationToken = default)
        {
            await _settingsService.SaveSettingsAsync(formID, data, cancellationToken);
        }
    }
}
