using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ProductCatalogue.IconNames;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InfraManager.UI.Web.Controllers.BFF.ProductCatalog;

[Authorize]
[ApiController]
[Route("bff/Icons")]
public class IconNameController : ControllerBase
{
    private const string Key = "icon_name_cache";
    private readonly IIconNameBLL _iconNameBLL;
    private readonly IMemoryCache _cache;

    public IconNameController(IIconNameBLL iconNameBLL, IMemoryCache cache)
    {
        _iconNameBLL = iconNameBLL;
        _cache = cache;
    }

    [HttpGet]
    public async Task<IconNameDetails[]> GetAllAsync(CancellationToken token)
    {
        if (!_cache.TryGetValue(Key, out IconNameDetails[] cached))
        {
            cached = await _iconNameBLL.GetIconsAsync(token);
            _cache.Set(Key, cached);
        }

        return cached;
    }

    [HttpGet("{id}")]
    public async Task<IconNameDetails> GetByIDAsync(Guid id, CancellationToken token)
    {
        return await _iconNameBLL.GetIconAsync(id, token);
    }
}