using InfraManager.BLL.ObjectIcons;
using InfraManager.BLL.ServiceDesk.MassIncidents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.MassIncidents
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MassIncidentTypesController : ControllerBase
    {
        private readonly IMassIncidentTypeBLL _service;
        private readonly IObjectIconBLL _iconsBll;

        public MassIncidentTypesController(IMassIncidentTypeBLL service, IObjectIconBLL iconsBll)
        {
            _service = service;
            _iconsBll = iconsBll;
        }

        [HttpGet]
        public async Task<MassIncidentTypeDetails[]> GetAsync([FromQuery]MassIncidentTypeListFilter filter, CancellationToken cancellationToken = default)
        {
            var details = await _service.GetDetailsPageAsync(filter, cancellationToken);

            //TODO: Костыль на время перехода, чтобы получить IconName типа массового инцидента
            //нужно использовать api/massIncidentTypes/{id}/icon
            foreach(var item in details)
            {
                var icon = await _iconsBll.GetAsync(
                    new InframanagerObject(item.IMObjID, ObjectClass.MassIncidentType), cancellationToken);
                item.IconName = icon.Name;
            }

            return details;
        }

        [HttpGet("{id}")]
        public async Task<MassIncidentTypeDetails> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var details = await _service.DetailsAsync(id, cancellationToken);

            //TODO: Перейти на другой апи ресурс и выпилить
            var icon = await _iconsBll.GetAsync(new InframanagerObject(details.IMObjID, ObjectClass.MassIncidentType), cancellationToken);
            details.IconName = icon.Name;

            return details;
        }

        [HttpPost]
        public async Task<MassIncidentTypeDetails> PostAsync([FromBody]MassIncidentTypeData data, CancellationToken cancellationToken = default)
        {
            var details = await _service.AddAsync(data, cancellationToken);

            //TODO: Перейти на другой апи ресурс и выпилить
            if (!string.IsNullOrWhiteSpace(data.IconName))
            {
                await _iconsBll.SetAsync(
                    new InframanagerObject(details.IMObjID, ObjectClass.MassIncidentType),
                    new ObjectIconData { Name = data.IconName },
                    cancellationToken);
            }

            return details;
        }

        [HttpPut("{id}")]
        public async Task<MassIncidentTypeDetails> PutAsync(int id, [FromBody]MassIncidentTypeData data, CancellationToken cancellationToken = default)
        {
            var details = await _service.UpdateAsync(id, data, cancellationToken);

            //TODO: Перейти на другой апи ресурс и выпилить
            if (!string.IsNullOrWhiteSpace(data.IconName))
            {
                await _iconsBll.SetAsync(
                    new InframanagerObject(details.IMObjID, ObjectClass.MassIncidentType),
                    new ObjectIconData { Name = data.IconName },
                    cancellationToken);
            }

            return details;
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            return _service.DeleteAsync(id, cancellationToken);
        }
    }
}
