using InfraManager.BLL.Parameters;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.Events;
using InfraManager.Services;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.BFF.Parameters
{
    /// <summary>
    /// Параметры
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ParameterEnumController : BaseApiController
    {
        private readonly IParameterEnumBLL _parameterEnumBLL;

        public ParameterEnumController(
            IParameterEnumBLL parameterEnumBLL)
        {
            _parameterEnumBLL = parameterEnumBLL;
        }
        
        [HttpGet]
        public async Task<ParameterEnumDetails[]> GetParameterEnumsAsync([FromQuery] ParameterEnumFilter filter, CancellationToken cancellationToken = default)
        {
            return await _parameterEnumBLL.GetParameterEnumsAsync(filter, cancellationToken);
        }
        
        [HttpGet("{id}")]
        public async Task<ParameterEnumDetails> GetParameterEnumAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _parameterEnumBLL.GetParameterEnumAsync(id, cancellationToken);
        }
        
        [HttpPost]
        public async Task<ParameterEnumDetails> AddParameterEnumAsync([FromBody]ParameterEnumDetails parameterData, CancellationToken cancellationToken = default)
        {
            return await _parameterEnumBLL.AddParameterEnumAsync(parameterData, cancellationToken);
        }
        
        [HttpPut]
        public async Task<ParameterEnumDetails> UpdateParameterEnumAsync([FromBody] ParameterEnumDetails parameterData, CancellationToken cancellationToken = default)
        {
            return await _parameterEnumBLL.UpdateParameterEnumAsync(parameterData, cancellationToken);
        }
        
        [HttpDelete("{id}")]
        public async Task DeleteParameterEnumAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _parameterEnumBLL.DeleteParameterEnumAsync(id, cancellationToken);
        }
        
        [HttpGet("parameterEnumValues")]
        public async Task<ParameterEnumValuesData[]> GetParameterEnumValuesAsync(Guid parameterEnumID, CancellationToken cancellationToken = default)
        {
            return await _parameterEnumBLL.GetParameterEnumValuesAsync(parameterEnumID, cancellationToken);
        }
        
        [HttpGet("parameterEnumValue")]
        public async Task<ParameterEnumValueData> GetParameterEnumValue(Guid id, CancellationToken cancellationToken = default)
        {
            return await _parameterEnumBLL.GetParameterEnumValueAsync(id, cancellationToken);
        }
        
        [HttpPut("parameterEnumValue")]
        public async Task UpdateParameterEnumValuesAsync([FromBody]List<ParameterEnumValuesData> parameterValuesData, CancellationToken cancellationToken = default)
        {
            await _parameterEnumBLL.UpdateParameterEnumValuesAsync(parameterValuesData, cancellationToken);
        }
        
        [HttpDelete("parameterEnumValue")]
        public async Task DeleteParameterEnumValueAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _parameterEnumBLL.DeleteParameterEnumValueAsync(id, cancellationToken);
        }
        
        [HttpGet("history")]
        public async Task<Event[]> GetHistoryAsync(Guid id, DateTime? dateFrom, DateTime? dateTill)
        {
            return await _parameterEnumBLL.GetHistoryAsync(id, dateFrom, dateTill);
        }
    }
}
