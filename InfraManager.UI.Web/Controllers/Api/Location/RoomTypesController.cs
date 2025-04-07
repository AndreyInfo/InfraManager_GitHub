using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL.Catalog;
using InfraManager.DAL.Location;
using InfraManager.BLL.Location.Rooms;

namespace InfraManager.UI.Web.Controllers.Api.Location;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RoomTypesController : ControllerBase
{
    private readonly IBasicCatalogBLL<RoomType, RoomTypeDetails, int,RoomType> _roomTypeCatalogBLL;

    public RoomTypesController(IBasicCatalogBLL<RoomType, RoomTypeDetails, int,RoomType> roomTypeCatalogBLL)
    {
        _roomTypeCatalogBLL = roomTypeCatalogBLL;
    }

    [HttpGet]
    public async Task<RoomTypeDetails[]> GetListTypesAsync([FromQuery] string search, CancellationToken cancellationToken)
    {
        return await _roomTypeCatalogBLL.GetListAsync(search, cancellationToken);
    }
}
