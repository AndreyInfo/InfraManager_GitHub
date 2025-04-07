using InfraManager.BLL.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.CrossPlatform.WebApi.Controllers
{
    [Authorize]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryDataProvider _historyDataProvider;

        public HistoryController(IHistoryDataProvider historyDataProvider)
        {
            _historyDataProvider = historyDataProvider;
        }

        [HttpGet]
        [Route("api/{classID}/{objectID}/events")]
        public async Task<HistoryItemDetails[]> HistoryListAsync([FromRoute] Guid objectID, [FromRoute] ObjectClass? classID,
            [FromQuery] HistoryFilter model, CancellationToken cancellationToken = default)
        {
            return await _historyDataProvider.GetHistoryByObjectAsync(objectID, classID, model, cancellationToken);
        }
    }
}
