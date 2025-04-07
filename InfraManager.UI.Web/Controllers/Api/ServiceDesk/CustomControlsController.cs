using AutoMapper;
using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.CustomControl;
using InfraManager.UI.Web.ResourceMapping;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class CustomControlsController : ControllerBase
    {
        private readonly ICustomControlBLL _service;
        private readonly IMapper _mapper;

        public CustomControlsController(ICustomControlBLL service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Отвечает на HTTP запрос GET api/{resource}/{id}/customControls/my
        /// </summary>
        /// <param name="resource">ИД ресурса (навание напр. calls, problems и т.п.)</param>
        /// <param name="objectID">ИД ресурса (идентификатор объекта) </param>
        /// <param name="cancellationToken"></param>
        /// <returns> Информация о том находится ли объект на контроле у текущего пользователя</returns>
        [HttpGet("{resource}/{objectID}/[controller]/my")]
        public async Task<ActionResult<CustomControlDetails>> GetMyAsync(
            WebApiResource resource,
            Guid objectID,
            CancellationToken cancellationToken = default)
        {
            if (!resource.TryGetObjectClass(out var classID))
            {
                return NotFound();
            }

            return await _service.GetCustomControlDetailsAsync(
                new InframanagerObject(objectID, classID),
                cancellationToken);
        }

        /// <summary>
        /// Отвечает на HTTP запрос POST api/{resource}/{id}/customControls/
        /// </summary>
        /// <param name="resource">ИД ресурса (навание напр. calls, problems и т.п.)</param>
        /// <param name="objectID">ИД ресурса (идентификатор объекта) </param>
        /// <param name="model">Данные о состоянии контроля у определенного пользователя</param>
        /// <param name="cancellationToken"></param>
        /// <returns> Информация о том находится ли объект на контроле у запрашиваемого пользователя</returns>
        [HttpPost("{resource}/{objectID}/[controller]")]
        public async Task<ActionResult<CustomControlDetails>> PostAsync(
            WebApiResource resource,
            Guid objectID,
            [FromBody] UserCustomControlDataModel model,
            CancellationToken cancellationToken = default) =>
            await UpdateAsync(resource, objectID, model, cancellationToken);
        

        /// <summary>
        /// Отвечает на HTTP запрос PUT api/{resource}/{id}/customControls/my
        /// </summary>
        /// <param name="resource">ИД ресурса (навание напр. calls, problems и т.п.)</param>
        /// <param name="objectID">ИД ресурса (идентификатор объекта) </param>
        /// <param name="model">Данные о состоянии контроля текущего пользователя</param>
        /// <param name="cancellationToken"></param>
        /// <returns> Информация о том находится ли объект на контроле у текущего пользователя</returns>
        [HttpPut("{resource}/{objectID}/[controller]/my")]
        public async Task<ActionResult<CustomControlDetails>> PutMyAsync(
            WebApiResource resource,
            Guid objectID,
            [FromBody] MyCustomControlDataModel model,
            CancellationToken cancellationToken = default) =>
            await UpdateAsync(resource, objectID, model, cancellationToken);

        private async Task<ActionResult<CustomControlDetails>> UpdateAsync(
            WebApiResource resource,
            Guid objectID,
            CustomControlDataModel model,
            CancellationToken cancellationToken = default)
        {
            if (!resource.TryGetObjectClass(out var classID))
            {
                return NotFound();
            }

            return await _service.SetCustomControlDetailsAsync(
                new InframanagerObject(objectID, classID),
                _mapper.Map<CustomControlData>(model),
                cancellationToken);
        }

        [HttpPost("[controller]/reports/underMyControl")]
        public async Task<ListItemModel[]> UnderMyControlReportAsync([FromBody]ListFilter filterBy, CancellationToken cancellationToken = default)
        {
            var items = await _service.GetListAsync(filterBy.ToServiceDeskFilter(), cancellationToken);

            return items.Select(x => _mapper.Map<ListItemModel>(x)).ToArray();
        }
    }
}
