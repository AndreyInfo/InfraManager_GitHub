using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.FormBuilder;
using InfraManager.BLL.FormBuilder.Contracts;
using InfraManager.BLL.FormBuilder.Forms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace InfraManager.UI.Web.Controllers.Api.FormBuilder;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FormsController : ControllerBase
{
    private readonly IFormBLL _formBuilderForm;
    
    public FormsController(IFormBLL formBLL)
    {
        _formBuilderForm = formBLL;
    }

    [HttpGet]
    public async Task<FormBuilderFormDetails[]> ListAsync([FromQuery] FormBuilderFilter filter,
        CancellationToken cancellationToken = default)
    {
        return await _formBuilderForm.ListAsync(filter, cancellationToken);
    }

    [HttpGet("{formID}")]
    public async Task<FormBuilderFormDetails> GetAsync([FromRoute] Guid formID,
        CancellationToken cancellationToken = default)
    {
        return await _formBuilderForm.GetAsync(formID, cancellationToken);
    }

    [HttpPost]
    public async Task<FormBuilderFormDetails> PostAsync([FromBody] FormBuilderFormData data,
        CancellationToken cancellationToken = default)
    {
        return await _formBuilderForm.AddAsync(data, cancellationToken);
    }
    
    [HttpPut("{formID}")]
    public async Task<FormBuilderFormDetails> PutAsync([FromRoute] Guid formID, [FromBody] FormBuilderFormData data,
        CancellationToken cancellationToken = default)
    {
        return await _formBuilderForm.UpdateAsync(formID, data, cancellationToken);
    }

    [HttpDelete("{formID}")]
    public async Task DeleteAsync([FromRoute] Guid formID, CancellationToken cancellationToken = default)
    {
        await _formBuilderForm.DeleteAsync(formID, cancellationToken);
    }

    [HttpPost("{formID}/publish")]
    public async Task PublishAsync([FromRoute] Guid formID, CancellationToken cancellationToken = default)
    {
        await _formBuilderForm.PublishAsync(formID, cancellationToken);
    }
    
    [HttpGet("AvailableTypes")]
    public FormBuilderClassType[] GetAvailableTypes()
    {
        return _formBuilderForm.GetAvailableTypes();
    }
}