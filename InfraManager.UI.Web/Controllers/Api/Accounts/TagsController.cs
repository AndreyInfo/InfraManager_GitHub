using InfraManager.BLL.Accounts;
using InfraManager.BLL.Accounts.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly ITagBLL _tagBLL;

    public TagsController(ITagBLL tagBLL)
    {
        _tagBLL = tagBLL;
    }

    [HttpGet]
    public Task<TagDetailsModel> GetAsync([FromQuery] TagFilter filter, CancellationToken cancellationToken = default)
    {
        
        return  _tagBLL.GetByUserAccountIDAsync(filter.UserAccountID, cancellationToken);
    }

    [HttpDelete]
    public async Task DeleteAsync(TagData tag, CancellationToken cancellationToken = default)
    {
        await _tagBLL.DeleteAsync(tag, cancellationToken);
    }

    [HttpPost]
    public async Task<TagDetails> CreateAsync(TagData tag, CancellationToken cancellationToken = default)
    {
        return await _tagBLL.CreateAsync(tag, cancellationToken);
    }

    [HttpPut]
    public async Task UpdateAsync([FromBody] TagDataModel tagData, CancellationToken cancellationToken = default)
    {
        await _tagBLL.UpdateAsync(tagData, cancellationToken);
    }

}