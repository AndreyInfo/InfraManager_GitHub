using InfraManager.BLL.Settings.UserFields;
using InfraManager.DAL.Settings.UserFields;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Settings;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserFieldsController : ControllerBase
{
    private readonly IServiceMapper<UserFieldType, IUserFieldNameBLL> _service;

    public UserFieldsController(
        IServiceMapper<UserFieldType, IUserFieldNameBLL> service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<UserFieldData[]> ListAsync(UserFieldType userFieldType, CancellationToken cancellationToken = default)
    {
        var visibleUserFields = await _service.Map(userFieldType).ListVisibleAsync(cancellationToken);

        return visibleUserFields
            .Select(x => new UserFieldData { Number = x.Key, Text = x.Value })
            .ToArray();
    }

    [HttpGet("nondefault")]
    public async Task<UserFieldData[]> ListNonDefaultAsync(UserFieldType userFieldType, CancellationToken cancellationToken = default)
    {
        var visibleUserFields = await _service.Map(userFieldType).ListNonDefaultAsync(cancellationToken);

        return visibleUserFields
            .Select(x => new UserFieldData { Number = x.Key, Text = x.Value })
            .ToArray();
    }

    [HttpPut("{userFieldType}/{number}")]
    public async Task<FieldNumber> PutAsync([FromRoute] UserFieldType userFieldType
        , [FromRoute] FieldNumber number
        , [FromBody] UserFieldData model
        , CancellationToken cancellationToken)
        => await _service.Map(userFieldType).UpdateAsync(number, model, cancellationToken);
}
