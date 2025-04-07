using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Import;
using InfraManager.ServiceBase.ImportService.DBService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.IM.ImportService.Controllers.Database
{
    [Route("api/Database/UIDBConnectionString")]
    [ApiController]
    public class UIDBConnectionStringController : ControllerBase
    {
        private readonly IImportApi _api;

        public UIDBConnectionStringController(IImportApi api)
        {
            _api = api;
        }

        [HttpGet("{id}")]
        public Task<UIDBConnectionStringOutputDetails> GetASync(Guid id, CancellationToken cancellationToken = default)
        {
            return _api.DbConnectionStringDetailsAsync(id, cancellationToken);
        }

        [HttpGet]
        public Task<UIDBConnectionStringOutputDetails[]> GetAsync([FromQuery] UIDBConnectionStringFilter filter,
            CancellationToken cancellationToken = default)
        {
            return _api.GetConnectionStringDetailsArrayAsync(filter, cancellationToken);
        }

        [HttpPost]
        public Task<UIDBConnectionStringOutputDetails> PostAsync([FromBody] UIDBConnectionStringData data,
            CancellationToken cancellationToken = default)
        {
            return _api.AddConnectionStringAsync(data, cancellationToken);
        }

        [HttpPut("{id}")]
        public Task<UIDBConnectionStringOutputDetails> PutAsync(Guid id, [FromBody] UIDBConnectionStringData data,
            CancellationToken cancellationToken = default)
        {
            return _api.UpdateConnectionStringAsync(id, data, cancellationToken);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _api.ConnectionStringDeleteAsync(id, cancellationToken);
        }
    }
}