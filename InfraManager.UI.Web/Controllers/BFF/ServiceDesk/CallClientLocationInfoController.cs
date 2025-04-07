using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Threading;
using InfraManager.BLL.ServiceDesk.Calls;
using InfraManager.UI.Web.Models.ServiceDesk;
using InfraManager.BLL.Location;
using System.Collections.Generic;
using InfraManager.BLL;
using InfraManager.DAL.ServiceDesk.Calls;

namespace InfraManager.UI.Web.Controllers.BFF.ServiceDesk
{
    [Route("bff/[controller]")]
    [ApiController]
    [Authorize]
    public class CallClientLocationInfoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IServiceMapper<ObjectClass, IQueryCallClientLocationInfo> _serviceMapper;
        public CallClientLocationInfoController(
                IMapper mapper,
                IServiceMapper<ObjectClass, IQueryCallClientLocationInfo> serviceMapper)
        {
            _mapper = mapper;
            _serviceMapper = serviceMapper;
        }

        [HttpGet]
        [Obsolete("Удалить, после того, как фронтенд будет использовать REST API")]
        public async Task<CallClientLocationInfoModel> GetAsync([FromBody] ClientLocationModel param, CancellationToken cancellationToken)
        {
            var infoDetail = await DetailsAsync(param, cancellationToken);
            var infoModel = _mapper.Map<CallClientLocationInfoModel>(infoDetail);
            var locationParts = new List<string>
            {
                string.IsNullOrEmpty(infoDetail.OrganizationName) ? "Нет" : infoDetail.OrganizationName,
                infoDetail.BuildingName,
                infoDetail.FloorName,
                infoDetail.RoomName
            };
            if (!string.IsNullOrEmpty(infoDetail.WorkplaceName))
            {
                locationParts.Add(infoDetail.WorkplaceName);
            }

            infoModel.PlaceFullName = string.Join(" \\ ", locationParts);
            
            return infoModel;
        }

        private async Task<CallClientLocationInfoDetails> DetailsAsync(ClientLocationModel param, CancellationToken cancellationToken)
        {
            var objectClass = (ObjectClass)param.LocationClassID;
            var infoItem = await _serviceMapper.Map(objectClass).GetCallClientLocationInfoAsync(param.LocationID, cancellationToken);
            if (infoItem == null)
            {
                throw new ObjectNotFoundException<Guid>(param.LocationID, $"not found {objectClass}");
            }

            return _mapper.Map<CallClientLocationInfoDetails>(infoItem);
        }
    }
}
